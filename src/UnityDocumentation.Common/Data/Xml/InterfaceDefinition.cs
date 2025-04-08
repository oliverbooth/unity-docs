using System.Xml.Serialization;

namespace UnityDocumentation.Common.Data.Xml;

/// <summary>
///     Denotes the name of an interface.
/// </summary>
public sealed class InterfaceDefinition
{
    /// <summary>
    ///     Gets or sets the name of the interface.
    /// </summary>
    /// <value>The name of the interface.</value>
    [XmlElement("InterfaceName")]
    public string Name { get; set; } = string.Empty;
}
