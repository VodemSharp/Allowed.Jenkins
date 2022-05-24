using System.Runtime.InteropServices;

namespace Allowed.Jenkins.Client.Helpers;

public static class FileHelper
{
    const int ERROR_SHARING_VIOLATION = 32;
    const int ERROR_LOCK_VIOLATION = 33;

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
        for (var i = 0; i < files.Length; i++)
        {
            var file = files[i];
            var temppath = Path.Combine(destDirName, file.Name);

            try
            {
                file.CopyTo(temppath, true);
            }
            catch (Exception ex)
            {
                // If file not locker throw exception
                var errorCode = Marshal.GetHRForException(ex) & ((1 << 16) - 1);
                if (ex is not IOException || errorCode is not (ERROR_SHARING_VIOLATION or ERROR_LOCK_VIOLATION))
                    throw;

                Task.Delay(100);
                i--;
            }
        }

        foreach (var subdir in dirs)
        {
            var temppath = Path.Combine(destDirName, subdir.Name);
            DirectoryCopy(subdir.FullName, temppath);
        }
    }
}