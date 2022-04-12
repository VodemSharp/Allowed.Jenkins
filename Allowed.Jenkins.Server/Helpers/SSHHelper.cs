using Renci.SshNet;

namespace Allowed.Jenkins.Server.Helpers;

public static class SSHHelper
{
    private static void CheckCommand(SshCommand command)
    {
        if (command.ExitStatus != 0 && !string.IsNullOrEmpty(command.Error))
            throw new Exception(command.Error);
    }

    public static string RunCommand(SshClient client, string command)
    {
        Console.WriteLine(command);

        var sshCommand = client.RunCommand(command);
        CheckCommand(sshCommand);

        Console.WriteLine(sshCommand.Result);
        return sshCommand.Result;
    }

    public static void UploadFile(SftpClient client, string localPath, string remotePath)
    {
        Console.WriteLine($"Uploading {localPath}");

        using Stream fileStream = new FileStream(localPath, FileMode.Open);
        client.UploadFile(fileStream, remotePath);
        
        Console.WriteLine($"Uploaded {localPath}");
    }

    public static void UploadDirectory(SftpClient client, string localPath, string remotePath)
    {
        var localFiles = new DirectoryInfo(localPath).EnumerateFileSystemInfos();
        var remoteFiles = client.ListDirectory(remotePath);

        foreach (var localFile in localFiles)
            if (localFile.Attributes.HasFlag(FileAttributes.Directory))
            {
                var subPath = remotePath + "/" + localFile.Name;
                if (!client.Exists(subPath))
                    client.CreateDirectory(subPath);

                UploadDirectory(client, localFile.FullName, remotePath + "/" + localFile.Name);
            }
            else
            {
                var remoteFile = remoteFiles.FirstOrDefault(f => f.Name == localFile.Name);

                if (remoteFile != null && localFile.LastWriteTimeUtc <= remoteFile.LastWriteTimeUtc &&
                    ((FileInfo) localFile).Length == remoteFile.Attributes.Size) continue;

                using Stream fileStream = new FileStream(localFile.FullName, FileMode.Open);

                var processName = remoteFile == null ? "Adding" : "Updating";
                Console.WriteLine($"{processName} {Path.GetRelativePath(localPath, localFile.FullName)}");

                client.UploadFile(fileStream, $"{remotePath}/{localFile.Name}");
            }
    }
}