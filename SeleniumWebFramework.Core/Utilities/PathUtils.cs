namespace SeleniumWebFramework.Core.Utilities;

public static class PathUtils
{
    /// <summary>
    /// Gets the application domain base directory where output binaries and assets reside.
    /// </summary>
    public static string GetBaseDirectory() => AppDomain.CurrentDomain.BaseDirectory;

    /// <summary>
    /// Locates project root or gracefully falls back to AppDomain BaseDirectory in compiled/container environments.
    /// </summary>
    public static string GetProjectRoot()
    {
        var currentDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

        while (currentDir != null)
        {
            if (currentDir.GetFiles("*.csproj").Length > 0 || currentDir.GetFiles("*.sln").Length > 0)
            {
                return currentDir.FullName;
            }
            currentDir = currentDir.Parent;
        }

        return AppDomain.CurrentDomain.BaseDirectory;
    }
}