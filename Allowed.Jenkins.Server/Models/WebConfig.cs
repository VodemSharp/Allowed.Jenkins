using System.Xml.Serialization;

namespace Allowed.Jenkins.Server.Models;

[XmlRoot(ElementName = "add")]
public class Add
{
    [XmlAttribute(AttributeName = "name")] public string Name { get; set; }
    [XmlAttribute(AttributeName = "path")] public string Path { get; set; }
    [XmlAttribute(AttributeName = "verb")] public string Verb { get; set; }

    [XmlAttribute(AttributeName = "modules")]
    public string Modules { get; set; }

    [XmlAttribute(AttributeName = "resourceType")]
    public string ResourceType { get; set; }
}

[XmlRoot(ElementName = "handlers")]
public class Handlers
{
    [XmlElement(ElementName = "add")] public Add Add { get; set; }
}

[XmlRoot(ElementName = "environmentVariable")]
public class EnvironmentVariable
{
    [XmlAttribute(AttributeName = "name")] public string Name { get; set; }

    [XmlAttribute(AttributeName = "value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "environmentVariables")]
public class EnvironmentVariables
{
    [XmlElement(ElementName = "environmentVariable")]
    public EnvironmentVariable EnvironmentVariable { get; set; }
}

[XmlRoot(ElementName = "aspNetCore")]
public class AspNetCore
{
    [XmlElement(ElementName = "environmentVariables")]
    public EnvironmentVariables EnvironmentVariables { get; set; }

    [XmlAttribute(AttributeName = "processPath")]
    public string ProcessPath { get; set; }

    [XmlAttribute(AttributeName = "arguments")]
    public string Arguments { get; set; }

    [XmlAttribute(AttributeName = "stdoutLogEnabled")]
    public string StdoutLogEnabled { get; set; }

    [XmlAttribute(AttributeName = "stdoutLogFile")]
    public string StdoutLogFile { get; set; }

    [XmlAttribute(AttributeName = "hostingModel")]
    public string HostingModel { get; set; }
}

[XmlRoot(ElementName = "system.webServer")]
public class SystemWebServer
{
    [XmlElement(ElementName = "handlers")] public Handlers Handlers { get; set; }

    [XmlElement(ElementName = "aspNetCore")]
    public AspNetCore AspNetCore { get; set; }
}

[XmlRoot(ElementName = "location")]
public class Location
{
    [XmlElement(ElementName = "system.webServer")]
    public SystemWebServer WebServer { get; set; }

    [XmlAttribute(AttributeName = "path")] public string Path { get; set; }

    [XmlAttribute(AttributeName = "inheritInChildApplications")]
    public string InheritInChildApplications { get; set; }
}

[XmlRoot(ElementName = "configuration")]
public class Configuration
{
    public string Version { get; set; }
    [XmlElement(ElementName = "location")] public Location Location { get; set; }
}