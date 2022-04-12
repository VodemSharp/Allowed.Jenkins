using Allowed.Jenkins.Common;

namespace Allowed.Jenkins.Server.Helpers;

public static class DotNetHelper
{
    public static async Task Restore(string projectPath)
    {
        await ProcessHelper.RunProcess("dotnet", $"restore \"{projectPath}\"");
    }

    public static async Task MSBuild(string projectPath)
    {
        await ProcessHelper.RunProcess("msbuild.exe",
            $"\"{projectPath}\" /nologo /nr:false /p:platform=\"x64\" /p:configuration=\"release\" /p:deployonbuild=true /t:clean;restore;rebuild");
    }
}