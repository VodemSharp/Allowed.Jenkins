using System.IO.Compression;
using Allowed.Jenkins.Server.Helpers;
using Renci.SshNet;

args = args.Select(a =>
{
    var temp = a.StartsWith("\"") ? a[1..] : a;
    return temp.EndsWith("\"") ? temp[..^1] : temp;
}).ToArray();

if (args[0] == "IIS")
{
    var workspace = args[1];
    var project = args[2];
    var environment = args[3];
    var siteName = args[4];
    var sitePath = args[5];
    var host = args[6];
    var username = args[7];
    var password = args[8];

    var buildPath = Path.Combine(workspace, project, "bin", "x64", "release", "net6.0");
    var publishPath = Path.Combine(buildPath, "publish");
    var projectPath = $"{workspace}\\{project}\\{project}.csproj";

    using (var client = new SshClient(host, username, password))
    {
        client.Connect();

        JenkinsClient.CreateJenkinsFolder(client, sitePath);
        await DotNetHelper.Restore(projectPath);
        await DotNetHelper.MSBuild(projectPath);
        WebConfigHelper.AddEnvironment(Path.Combine(publishPath, "web.config"), environment);

        client.Disconnect();
    }

    using (var client = new SftpClient(host, username, password))
    {
        client.Connect();

        if (File.Exists(Path.Combine(buildPath, "publish.zip")))
            File.Delete(Path.Combine(buildPath, "publish.zip"));

        ZipFile.CreateFromDirectory(Path.Combine(buildPath, "publish"), Path.Combine(buildPath, "publish.zip"),
            CompressionLevel.Optimal, false);
        SSHHelper.UploadFile(client, Path.Combine(buildPath, "publish.zip"),
            $"/{Path.Combine(sitePath, "jenkins", "publish.zip")}");

        client.Disconnect();
    }

    using (var client = new SshClient(host, username, password))
    {
        client.Connect();

        JenkinsClient.StopSite(client, siteName);
        JenkinsClient.UnZipAndMove(client, sitePath);
        JenkinsClient.StartSite(client, siteName);

        JenkinsClient.RemoveJenkinsFolder(client, sitePath);

        client.Disconnect();
    }
}
else if (args[0] == "WS")
{
    var workspace = args[1];
    var project = args[2];
    var siteName = args[3];
    var sitePath = args[4];
    var host = args[5];
    var username = args[6];
    var password = args[7];

    var releasePath = Path.Combine(workspace, project, "bin", "x64", "release");
    var buildPath = Path.Combine(releasePath, "net6.0");
    var projectPath = $"{workspace}\\{project}\\{project}.csproj";

    using (var client = new SshClient(host, username, password))
    {
        client.Connect();

        JenkinsClient.CreateJenkinsFolder(client, sitePath);
        await DotNetHelper.Restore(projectPath);
        await DotNetHelper.MSBuild(projectPath);

        client.Disconnect();
    }

    using (var client = new SftpClient(host, username, password))
    {
        client.Connect();

        if (File.Exists(Path.Combine(releasePath, "publish.zip")))
            File.Delete(Path.Combine(releasePath, "publish.zip"));

        ZipFile.CreateFromDirectory(buildPath, Path.Combine(releasePath, "publish.zip"),
            CompressionLevel.Optimal, false);
        SSHHelper.UploadFile(client, Path.Combine(releasePath, "publish.zip"),
            $"/{Path.Combine(sitePath, "jenkins", "publish.zip")}");

        client.Disconnect();
    }

    using (var client = new SshClient(host, username, password))
    {
        client.Connect();

        JenkinsClient.StopService(client, siteName);
        JenkinsClient.UnZipAndMove(client, sitePath);
        JenkinsClient.StartService(client, siteName);

        JenkinsClient.RemoveJenkinsFolder(client, sitePath);

        client.Disconnect();
    }
}