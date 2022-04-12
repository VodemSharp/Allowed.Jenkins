using System.Diagnostics;

namespace Allowed.Jenkins.Common;

public static class ProcessHelper
{
    public static async Task<string> RunProcess(string fileName, string arguments)
    {
        var output = new List<string>();
        
        var process = new Process();
        process.StartInfo.FileName = fileName;
        process.StartInfo.Arguments = arguments;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.RedirectStandardOutput = true;
        process.OutputDataReceived += (sender, e) =>
        {
            output.Add(e.Data);
            Console.WriteLine(e.Data);
        };

        process.Start();
        process.BeginOutputReadLine();
        await process.WaitForExitAsync();

        return string.Join("\n", output.Where(o => !string.IsNullOrEmpty(o)));
    }
}