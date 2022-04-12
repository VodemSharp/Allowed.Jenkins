using System.Xml;
using System.Xml.Serialization;
using Allowed.Jenkins.Server.Models;

namespace Allowed.Jenkins.Server.Helpers;

public static class WebConfigHelper
{
    public static void AddEnvironment(string path, string environment)
    {
        var ns = new XmlSerializerNamespaces();
        ns.Add("", "");

        var xmlSerializer = new XmlSerializer(typeof(Configuration));
        Configuration configuration;

        using (var fs = new FileStream(path, FileMode.Open))
        {
            configuration = (Configuration) xmlSerializer.Deserialize(fs);
        }

        configuration.Location.WebServer.AspNetCore.EnvironmentVariables = new EnvironmentVariables
        {
            EnvironmentVariable = new EnvironmentVariable
            {
                Name = "ASPNETCORE_ENVIRONMENT",
                Value = environment
            }
        };

        using (var writer = new StreamWriter(path))
        using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings {Indent = true}))
        {
            xmlSerializer.Serialize(xmlWriter, configuration, ns);
        }
    }
}