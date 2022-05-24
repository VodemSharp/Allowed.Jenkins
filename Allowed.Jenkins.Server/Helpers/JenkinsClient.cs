using Renci.SshNet;

namespace Allowed.Jenkins.Server.Helpers;

public static class JenkinsClient
{
    private static readonly string _jenkinsClient = "C:\\Allowed.Jenkins.Client\\Allowed.Jenkins.Client.exe";

    public static void CreateJenkinsFolder(SshClient client, string sitePath)
    {
        SSHHelper.RunCommand(client, $"{_jenkinsClient} CreateJenkinsFolder \"{sitePath}\"");
    }

    public static void RemoveJenkinsFolder(SshClient client, string sitePath)
    {
        SSHHelper.RunCommand(client, $"{_jenkinsClient} RemoveJenkinsFolder \"{sitePath}\"");
    }

    public static void StartSite(SshClient client, string siteName)
    {
        SSHHelper.RunCommand(client, $"{_jenkinsClient} StartSite \"{siteName}\"");
    }

    public static void StopSite(SshClient client, string siteName)
    {
        SSHHelper.RunCommand(client, $"{_jenkinsClient} StopSite \"{siteName}\"");
    }
    
    public static void StartService(SshClient client, string serviceName)
    {
        SSHHelper.RunCommand(client, $"{_jenkinsClient} StartService \"{serviceName}\"");
    }

    public static void StopService(SshClient client, string serviceName)
    {
        SSHHelper.RunCommand(client, $"{_jenkinsClient} StopService \"{serviceName}\"");
    }

    public static void UnZipAndMove(SshClient client, string sitePath)
    {
        SSHHelper.RunCommand(client, $"{_jenkinsClient} UnZipAndMove \"{sitePath}\"");
    }
}