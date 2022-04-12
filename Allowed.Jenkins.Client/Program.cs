using System.IO.Compression;
using Allowed.Jenkins.Client.Helpers;
using Allowed.Jenkins.Client.IIS;

switch (args[0])
{
    // args[1] - site name
    case "StopSite":
        await IISCommands.ChangeAppCmdState(args[1], AppCmdType.Site, false);
        await IISCommands.ChangeAppCmdState(args[1], AppCmdType.AppPool, false);
        break;

    case "StartSite":
        await IISCommands.ChangeAppCmdState(args[1], AppCmdType.AppPool, true);
        await IISCommands.ChangeAppCmdState(args[1], AppCmdType.Site, true);
        break;
    //

    // args[1] - site path
    case "CreateJenkinsFolder":
        var jenkinsFolderCreate = $"{args[1]}\\jenkins";

        if (Directory.Exists(jenkinsFolderCreate))
            Directory.Delete(jenkinsFolderCreate, true);

        Directory.CreateDirectory(jenkinsFolderCreate);
        break;

    case "RemoveJenkinsFolder":
        var jenkinsFolderRemove = $"{args[1]}\\jenkins";

        if (Directory.Exists(jenkinsFolderRemove))
            Directory.Delete(jenkinsFolderRemove, true);
        break;

    case "UnZipAndMove":
        var jenkinsFolderZip = $"{args[1]}\\jenkins";
        var publishZip = Path.Combine(jenkinsFolderZip, "publish.zip");

        ZipFile.ExtractToDirectory(publishZip, jenkinsFolderZip);
        File.Delete(publishZip);

        FileHelper.DirectoryCopy(jenkinsFolderZip, args[1]);

        break;
    //
}