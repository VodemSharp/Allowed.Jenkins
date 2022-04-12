using Allowed.Jenkins.Common;

namespace Allowed.Jenkins.Client.IIS;

public static class IISCommands
{
    public static async Task ChangeAppCmdState(string siteName, AppCmdType type, bool start)
    {
        var existsResult = await ProcessHelper.RunProcess("C:\\Windows\\system32\\inetsrv\\appcmd.exe",
            $"list {(type == AppCmdType.AppPool ? "apppool" : "sites")} /name:\"{siteName}\" /state:\"{(start ? "Stopped" : "Started")}\"");

        if (!string.IsNullOrEmpty(existsResult))
            await ProcessHelper.RunProcess("C:\\Windows\\system32\\inetsrv\\appcmd.exe",
                $"{(start ? "start" : "stop")} {(type == AppCmdType.AppPool ? "apppool" : "site")} \"{siteName}\"");
    }
}