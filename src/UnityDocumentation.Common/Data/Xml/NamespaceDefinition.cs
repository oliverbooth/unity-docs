using System.Xml.Serialization;

namespace UnityDocumentation.Common.Data.Xml;

[XmlRoot("Namespace")]
public sealed class NamespaceDefinition
{
    /// <summary>
    ///     Gets or sets the documentation of the namespace.
    /// </summary>
    /// <value>The documentation of the namespace.</value>
    [XmlElement("Docs")]
    public DocumentationDefinition Documentation { get; set; } = new();

    /// <summary>
    ///     Gets or sets the name of the namespace.
    /// </summary>
    /// <value>The name of the namespace.</value>
    [XmlAttribute("Name")]
    public string Name { get; set; } = string.Empty;
}
