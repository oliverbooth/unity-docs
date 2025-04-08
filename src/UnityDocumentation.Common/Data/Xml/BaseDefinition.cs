using System.Xml.Serialization;

namespace UnityDocumentation.Common.Data.Xml;

/// <summary>
///     Denotes the base type of a type.
/// </summary>
public sealed class BaseDefinition
{
    /// <summary>
    ///     Gets or sets the name of the base type.
    /// </summary>
    /// <value>The name of the base type.</value>
    [XmlElement("BaseTypeName")]
    public string TypeName { get; set; } = string.Empty;
}
