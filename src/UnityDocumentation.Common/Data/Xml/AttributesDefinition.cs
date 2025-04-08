using System.Xml.Serialization;

namespace UnityDocumentation.Common.Data.Xml;

/// <summary>
///     Contains the attribute definitions of a type.
/// </summary>
public sealed class AttributesDefinition
{
    /// <summary>
    ///     Gets or sets the attribute definitions of the type.
    /// </summary>
    [XmlAttribute("Attribute")]
    public AttributeDefinition[] Attributes { get; set; } = [];
}
