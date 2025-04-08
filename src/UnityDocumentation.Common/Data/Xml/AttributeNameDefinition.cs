using System.Xml.Serialization;

namespace UnityDocumentation.Common.Data.Xml;

/// <summary>
///     Denotes the attribute definition.
/// </summary>
public sealed class AttributeNameDefinition
{
    /// <summary>
    ///     Gets or sets the language of the attribute.
    /// </summary>
    /// <value>The language of the attribute.</value>
    [XmlAttribute("Language")]
    public string Language { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the name of the attribute.
    /// </summary>
    /// <value>The name of the attribute.</value>
    [XmlText]
    public string Name { get; set; } = string.Empty;
}
