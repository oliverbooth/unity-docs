using System.Xml.Serialization;

namespace UnityDocumentation.Common.Data.Xml;

/// <summary>
///     Contains parameter documentation definition.
/// </summary>
public sealed class ParameterDocumentationDefinition
{
    /// <summary>
    ///     Gets or sets the name of the parameter.
    /// </summary>
    /// <value>The name of the parameter.</value>
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the summary of the documentation.
    /// </summary>
    /// <value>The summary of the documentation.</value>
    [XmlText]
    public string Summary { get; set; } = string.Empty;
}
