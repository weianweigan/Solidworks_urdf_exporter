using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SW2URDF.Properties;
using SW2URDF.UI;
using SW2URDF.URDFExport;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.Base.Enums;
using Xarial.XCad.SolidWorks;
using Xarial.XCad.UI.Commands;

namespace SW2URDF;

[ComVisible(true)]
[Guid("1398B27F-E519-4037-98A7-B4A0DF144AE5")]
[Title("Solidworks to urdf exporter")]
[Icon(typeof(Resources), nameof(Resources.logo))]
public sealed class SwAddin : SwAddInEx
{
    private static readonly log4net.ILog logger = Utilities.Logger.GetLogger();

    public override void OnConnect()
    {
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        CommandManager.AddCommandGroup<Commands>().CommandClick += SwAddin_CommandClick;
    }

    private void SwAddin_CommandClick(Commands spec)
    {
        try
        {
            switch (spec)
            {
                case Commands.PartExporter:
                    PartExporter(Application.Sw);
                    break;
                case Commands.AssemblyExporter:
                    AssemblyExporter(Application.Sw);
                    break;
                case Commands.Axis:
                    RunAxisCommand();
                    break;
                case Commands.Coord:
                    RunCoordCommand();
                    break;
                default:
                    throw new NotSupportedException($"Not support command: {spec}");
            }
        }
        catch (Exception ex)
        {
            logger.Error($"{spec} error: {0}", ex);
            Application.ShowMessageBox(ex.Message, MessageBoxIcon_e.Error);
        }
    }

    private void RunCoordCommand()
    {
        // swCommands_CoordinateSystem 154; valid for parts with a selected sketch; Features toolbar > Reference Geometry > Coordinate System
        RunCommand(154, Resources.ReferenceCoord);
    }

    private void RunAxisCommand()
    {
        // swCommands_Axis 22; valid for parts; Reference Geometry toolbar > Axis or Features toolbar > Reference Geometry > Axis
        RunCommand(22, Resources.ReferenceAxis);
    }

    private void RunCommand(int id, string title)
    {
        var doc = Application.Sw.IActiveDoc2;
        if (doc == null)
        {
            return;
        }

        swDocumentTypes_e docType = (swDocumentTypes_e)doc.GetType();
        if (docType is swDocumentTypes_e.swDocPART or swDocumentTypes_e.swDocASSEMBLY)
        {
            doc.Extension.RunCommand(id, title);
        }
        else
        {
            throw new NotSupportedException($"Not support document type: {docType}");
        }
    }

    internal void AssemblyExporter(ISldWorks app)
    {
        var doc = (ModelDoc2)app.ActiveDoc;
        logger.Info("Assembly export called for file " + doc.GetTitle());
        bool saveAndRebuild = false;
        if (doc.GetSaveFlag())
        {
            saveAndRebuild = true;
            logger.Info("Save is required");
        }
        else if (
            doc.Extension.NeedsRebuild2
            != (int)swModelRebuildStatus_e.swModelRebuildStatus_FullyRebuilt
        )
        {
            saveAndRebuild = true;
            logger.Info("A rebuild is required");
        }
        if (
            saveAndRebuild
            || Application.ShowMessageBox(
                Resources.PromptAssemblySaveAndRebuild,
                MessageBoxIcon_e.Question,
                MessageBoxButtons_e.YesNo
            ) == MessageBoxResult_e.Yes
        )
        {
            int options =
                (int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced
                | (int)swSaveAsOptions_e.swSaveAsOptions_Silent;
            logger.Info("Saving assembly");
            doc.Save3(options, 0, 0);

            logger.Info("Opening property manager");
            SetupPropertyManager();
        }

        void SetupPropertyManager()
        {
            ExportPropertyManager pm = new((SldWorks)app);
            logger.Info("Loading config tree");
            bool success = pm.LoadConfigTree();

            if (success)
            {
                logger.Info("Showing property manager");
                pm.Show();
            }
        }
    }

    internal void PartExporter(ISldWorks app)
    {
        logger.Info("Part export called");
        ModelDoc2 modeldoc = (ModelDoc2)app.ActiveDoc;
        if (
            (modeldoc.Extension.NeedsRebuild2 == 0)
            || Application.ShowMessageBox(
                Resources.PromptPartSave,
                MessageBoxIcon_e.Question,
                MessageBoxButtons_e.YesNo
            ) == MessageBoxResult_e.Yes
        )
        {
            if (modeldoc.Extension.NeedsRebuild2 != 0)
            {
                int options =
                    (int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced
                    | (int)swSaveAsOptions_e.swSaveAsOptions_Silent;
                logger.Info("Saving part");
                modeldoc.Save3(options, 0, 0);
            }

            PartExportForm exportForm = new((SldWorks)app);
            logger.Info("Showing part");
            exportForm.Show();
        }
    }

    private Assembly? CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
        AssemblyName assemblyName = new AssemblyName(args.Name);

        var dir = Path.GetDirectoryName(typeof(SwAddin).Assembly.Location);
        var dllLocation = Path.Combine(dir, assemblyName.Name + ".dll");

        if (File.Exists(dllLocation))
            return Assembly.LoadFrom(dllLocation);

        return null;
    }
}
