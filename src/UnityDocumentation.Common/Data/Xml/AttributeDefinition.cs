using System.Xml.Serialization;

namespace UnityDocumentation.Common.Data.Xml;

/// <summary>
///     Represents an attribute definition.
/// </summary>
public sealed class AttributeDefinition
{
    /// <summary>
    ///     Gets or sets the assemblies in which the attribute is defined.
    /// </summary>
    [XmlAttribute("AttributeName")]
    public AttributeNameDefinition[] Definitions { get; set; } = [];
}
