using System.Diagnostics;
using System.Runtime.CompilerServices;
using SW2URDF.Utilities;

[assembly: InternalsVisibleTo("TestRunner")]

namespace SW2URDF.Versioning;

internal class Version
{
    public static string GetCommitVersion()
    {
        // Getting commit version which is attached to the latest git commit
        return FileVersionInfo.GetVersionInfo(typeof(Logger).Assembly.Location).ProductVersion;
    }

    public static string GetBuildVersion()
    {
        // Getting AssemblyVersion which is auto incremented for each build. See the AssemblyInfo.cs
        return typeof(Logger).Assembly.GetName().Version.ToString();
    }
}
