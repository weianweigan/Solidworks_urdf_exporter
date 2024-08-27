using SW2URDF.Properties;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.UI.Commands.Attributes;
using Xarial.XCad.UI.Commands.Enums;

namespace SW2URDF;

[Title("Ros")]
[Icon(typeof(Resources), nameof(Resources.logo))]
public enum Commands
{
    [Title(typeof(Resources), nameof(Resources.ReferenceAxis))]
    [Icon(typeof(Resources), nameof(Resources.axis))]
    [CommandItemInfo(true, true, WorkspaceTypes_e.Part | WorkspaceTypes_e.Assembly, true)]
    Axis,

    [Title(typeof(Resources), nameof(Resources.ReferenceCoord))]
    [Icon(typeof(Resources), nameof(Resources.coord))]
    [CommandItemInfo(true, true, WorkspaceTypes_e.Part | WorkspaceTypes_e.Assembly, true)]
    Coord,

    [CommandSpacer]
    [Title(typeof(Resources), nameof(Resources.PartExporter))]
    [Icon(typeof(Resources), nameof(Resources.partexportericon))]
    [CommandItemInfo(true, true, WorkspaceTypes_e.Part, true)]
    PartExporter,

    [Title(typeof(Resources), nameof(Resources.AssemblyExporter))]
    [Icon(typeof(Resources), nameof(Resources.assemblyexportericon))]
    [CommandItemInfo(true, true, WorkspaceTypes_e.Assembly, true)]
    AssemblyExporter,
}
