using System.Xml.Serialization;

namespace UnityDocumentation.Common.Data.Xml;

/// <summary>
///     Contains the documentation definition.
/// </summary>
public sealed class DocumentationDefinition
{
    /// <summary>
    ///     Gets or sets the parameters of the documentation.
    /// </summary>
    /// <value>The parameters of the documentation.</value>
    [XmlElement("param", Order = 0)]
    public ParameterDocumentationDefinition[] Parameters { get; set; } = [];

    /// <summary>
    ///     Gets or sets the summary of the documentation.
    /// </summary>
    /// <value>The summary of the documentation.</value>
    [XmlElement("summary", Order = 1)]
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the summary of the documentation.
    /// </summary>
    /// <value>The summary of the documentation.</value>
    [XmlElement("remarks", Order = 2)]
    public string? Remarks { get; set; }
}