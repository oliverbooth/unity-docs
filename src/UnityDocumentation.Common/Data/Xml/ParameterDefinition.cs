using System.Xml.Serialization;

namespace UnityDocumentation.Common.Data.Xml;

/// <summary>
///     Represents the parameters of a method.
/// </summary>
public sealed class ParametersDefinition
{
    /// <summary>
    ///     Gets or sets the parameters of the method.
    /// </summary>
    /// <value>The parameters of the method.</value>
    [XmlElement("Parameter")]
    public ParameterDefinition[] Parameters { get; set; } = [];
}

/// <summary>
///     Represents a parameter definition.
/// </summary>
public sealed class ParameterDefinition
{
    /// <summary>
    ///     Gets or sets the name of the parameter.
    /// </summary>
    /// <value>The name of the parameter.</value>
    [XmlAttribute("Name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the type of the parameter.
    /// </summary>
    /// <value>The type of the parameter.</value>
    [XmlAttribute("Type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the index of the parameter.
    /// </summary>
    /// <value>The index of the parameter.</value>
    [XmlAttribute("Index")]
    public int Index { get; set; }
}
