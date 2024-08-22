using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Deployment.WindowsInstaller;
using WixSharp;

namespace Installer;

internal class Program
{
    const string ProductName = "SolidworksURDFExporter";
    const string CompanyName = "Ros";
    const string ProductId = "6B987FA1-EBB0-4C20-BA70-ED4B9A11B5A6";
    const string OutputDir = "output";
    const string Manufacturer = "https://github.com/weianweigan/solidworks_urdf_exporter";
    const string HelpLink = "https://github.com/weianweigan/solidworks_urdf_exporter";
    const string InstallationDir = $@"%ProgramFiles%\{CompanyName}\{ProductName}";

    static void Main()
    {
        string BinaryDir = GetBinDir();
        Console.WriteLine($"Binary directory: {BinaryDir}");
        string Version = GetVersion();
        Console.WriteLine($"Version: {Version}");
        string FileName = $"{ProductName}-{Version}";

        var project = new Project(
            ProductName,
            new InstallDir(InstallationDir, GetWixEntities(BinaryDir)),
            new ElevatedManagedAction(
                CustomActions.InstallServer,
                Return.check,
                When.After,
                Step.InstallFiles,
                Condition.NOT_Installed
            ),
            new ElevatedManagedAction(
                CustomActions.UnInstallServer,
                Return.check,
                When.Before,
                Step.RemoveFiles,
                Condition.BeingUninstalled
            )
        )
        {
            OutDir = OutputDir,
            Platform = Platform.x64,
            UI = WUI.WixUI_InstallDir,
            Version = new Version(Version),
            OutFileName = FileName,
            InstallScope = InstallScope.perMachine,
            InstallPrivileges = InstallPrivileges.elevated,
            MajorUpgrade = MajorUpgrade.Default,
            GUID = new Guid(ProductId),
            BackgroundImage = @"Resources\Icons\BackgroundImage.png",
            BannerImage = @"Resources\Icons\BannerImage.png",
            ControlPanelInfo =
            {
                Manufacturer = Manufacturer,
                HelpLink = HelpLink,
                Comments = "DMS",
                ProductIcon = @"Resources\Icons\ShellIcon.ico"
            },
            ValidateBackgroundImage = false,
        };

        project.BuildMsi();
    }

    static string GetVersion() => typeof(Program).Assembly.GetName().Version.ToString();

    static string GetBinDir()
    {
        var dir = Path.GetDirectoryName(typeof(Program).Assembly.Location);

        while (!Directory.GetDirectories(dir).Any(p => p.EndsWith(".git")))
        {
            dir = Directory.GetParent(dir).FullName;
        }

        return Path.Combine(dir, "SW2URDF", "bin",
#if DEBUG
            "Debug"
#else
            "Release"
#endif
        );
    }

    static WixEntity[] GetWixEntities(string binaryDir)
    {
        return
        [
            (new Files($@"{binaryDir}\*.*")),
            new Dir(
                $@"%ProgramMenu%\{CompanyName}\{ProductName}",
                new ExeFileShortcut(
                    $"UnInstall {ProductName}",
                    "[System64Folder]msiexec.exe",
                    $"/x [ProductCode]"
                )
            )
        ];
    }
}

public class CustomActions
{
    [CustomAction]
    public static ActionResult InstallServer(Session session)
    {
        return session.HandleErrors(() =>
        {
            string batFile = Path.Combine(session.Property("INSTALLDIR"), "Register.bat");
            Process.Start(batFile);
        });
    }

    [CustomAction]
    public static ActionResult UnInstallServer(Session session)
    {
        return session.HandleErrors(() =>
        {
            string batFile = Path.Combine(session.Property("INSTALLDIR"), "UnRegister.bat");
            Process.Start(batFile);
        });
    }
}
