using System.Xml.Serialization;

namespace UnityDocumentation.Common.Data.Xml;

/// <summary>
///     Denotes the signature of a member or type.
/// </summary>
public sealed class SignatureDefinition
{
    /// <summary>
    ///     Gets or sets the language of the member or type signature.
    /// </summary>
    /// <value>The language of the member or type signature.</value>
    [XmlAttribute("Language")]
    public string Language { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the value of the member or type signature.
    /// </summary>
    /// <value>The value of the member or type signature.</value>
    [XmlAttribute("Value")]
    public string Value { get; set; } = string.Empty;
}
