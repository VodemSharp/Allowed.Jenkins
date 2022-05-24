using System.IO.Compression;
using System.ServiceProcess;
using Allowed.Jenkins.Client.Helpers;
using Microsoft.Web.Administration;

var mode = args[0];

switch (mode)
{
// args[1] - site name
    case "StopSite":
    {
        var serverManager = new ServerManager();
        var site = serverManager.Sites[args[1]];
        var pool = serverManager.ApplicationPools[args[1]];

        if (site.State is ObjectState.Started or ObjectState.Starting)
            site.Stop();
        
        if (pool.State is ObjectState.Started or ObjectState.Starting)
            pool.Stop();
        break;
    }
    case "StartSite":
    {
        var serverManager = new ServerManager();
        var site = serverManager.Sites[args[1]];
        var pool = serverManager.ApplicationPools[args[1]];

        if (pool.State is ObjectState.Stopped or ObjectState.Stopping)
            pool.Start();
        
        if (site.State is ObjectState.Stopped or ObjectState.Stopping)
            site.Start();
        break;
    }

// args[1] - service name
    case "StopService":
    {
        var service = new ServiceController(args[1]);
        if (service.CanStop)
            service.Stop();
        break;
    }
    case "StartService":
    {
        var service = new ServiceController(args[1]);
        if (service.Status is ServiceControllerStatus.Stopped or ServiceControllerStatus.StopPending)
            service.Start();
        break;
    }

// args[1] - site path
    case "CreateJenkinsFolder":
    {
        var jenkinsFolder = $"{args[1]}\\jenkins";

        if (Directory.Exists(jenkinsFolder))
            Directory.Delete(jenkinsFolder, true);

        Directory.CreateDirectory(jenkinsFolder);
        break;
    }
    case "RemoveJenkinsFolder":
    {
        var jenkinsFolder = $"{args[1]}\\jenkins";

        if (Directory.Exists(jenkinsFolder))
            Directory.Delete(jenkinsFolder, true);
        break;
    }
    case "UnZipAndMove":
    {
        var jenkinsFolderZip = $"{args[1]}\\jenkins";
        var publishZip = Path.Combine(jenkinsFolderZip, "publish.zip");

        ZipFile.ExtractToDirectory(publishZip, jenkinsFolderZip);
        File.Delete(publishZip);

        FileHelper.DirectoryCopy(jenkinsFolderZip, args[1]);
        break;
    }
}