namespace Allowed.Jenkins.Client.Helpers;

public static class FileHelper
{
    public static void DirectoryCopy(string sourceDirName, string destDirName)
    {
        // Get the subdirectories for the specified directory.
        var dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);

        var dirs = dir.GetDirectories();
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(destDirName)) Directory.CreateDirectory(destDirName);

        // Get the files in the directory and copy them to the new location.
        var files = dir.GetFiles();
        foreach (var file in files)
        {
            var temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, true);
        }

        foreach (var subdir in dirs)
        {
            var temppath = Path.Combine(destDirName, subdir.Name);
            DirectoryCopy(subdir.FullName, temppath);
        }
    }
}