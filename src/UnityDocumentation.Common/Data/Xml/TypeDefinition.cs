using System.Xml.Serialization;

namespace UnityDocumentation.Common.Data.Xml;

/// <summary>
///     Represents a type definition.
/// </summary>
[XmlRoot("Type")]
public sealed class TypeDefinition
{
    /// <summary>
    ///     Gets or sets the assemblies in which the type is defined.
    /// </summary>
    /// <value>The assemblies in which the type is defined.</value>
    [XmlElement("AssemblyInfo", Order = 1)]
    public AssemblyInfoDefinition[] Assemblies { get; set; } = [];

    /// <summary>
    ///     Gets or sets the base type definition of the type.
    /// </summary>
    /// <value>The base type definition.</value>
    [XmlElement("Base", Order = 2)]
    public BaseDefinition Base { get; set; } = new();

    /// <summary>
    ///     Gets or sets the documentation of the type.
    /// </summary>
    /// <value>The documentation of the type.</value>
    [XmlElement("Docs", Order = 5)]
    public DocumentationDefinition Documentation { get; set; } = new();

    /// <summary>
    ///     Gets or sets the members of the type.
    /// </summary>
    /// <value>The members of the type.</value>
    [XmlElement("Members", Order = 6)]
    public MembersDefinition Members { get; set; } = new();

    /// <summary>
    ///     Gets or sets the interfaces of the type.
    /// </summary>
    /// <value>The interfaces of the type.</value>
    [XmlElement("Interfaces", Order = 3)]
    public InterfacesDefinition Interfaces { get; set; } = new();

    /// <summary>
    ///     Gets or sets the signatures of the type.
    /// </summary>
    /// <value>The signatures of the type.</value>
    [XmlElement("TypeSignature", Order = 0)]
    public SignatureDefinition[] Signatures { get; set; } = [];

    /// <summary>
    ///     Gets the name of the type.
    /// </summary>
    /// <value>The name of the type.</value>
    [XmlAttribute("Name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the full name of the type.
    /// </summary>
    /// <value>The full name of the type.</value>
    [XmlAttribute("FullName")]
    public string FullName { get; set; } = string.Empty;
}
