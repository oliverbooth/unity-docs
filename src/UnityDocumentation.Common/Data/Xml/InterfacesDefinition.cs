using System.Xml.Serialization;

namespace UnityDocumentation.Common.Data.Xml;

/// <summary>
///     Denotes the interfaces of a type.
/// </summary>
public sealed class InterfacesDefinition
{
    /// <summary>
    ///     Gets or sets the interfaces of the type.
    /// </summary>
    /// <value>The interfaces of the type.</value>
    [XmlElement("Interface")]
    public InterfaceDefinition[] Interfaces { get; set; } = [];
}
