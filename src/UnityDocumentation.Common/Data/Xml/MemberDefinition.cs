using System.Reflection;
using System.Xml.Serialization;

namespace UnityDocumentation.Common.Data.Xml;

/// <summary>
///     Denotes a member definition.
/// </summary>
public sealed class MemberDefinition
{
    /// <summary>
    ///     Gets or sets the assemblies in which the member is defined.
    /// </summary>
    /// <value>The assemblies in which the member is defined.</value>
    [XmlElement("AssemblyInfo", Order = 2)]
    public AssemblyInfoDefinition[] Assemblies { get; set; } = [];

    /// <summary>
    ///     Gets or sets the documentation of the type.
    /// </summary>
    /// <value>The documentation of the type.</value>
    [XmlElement("Docs", Order = 4)]
    public DocumentationDefinition Documentation { get; set; } = new();

    /// <summary>
    ///     Gets or sets the parameters of the member.
    /// </summary>
    /// <value>The parameters of the member.</value>
    [XmlElement("Parameters", Order = 3)]
    public ParametersDefinition? Parameters { get; set; }

    /// <summary>
    ///     Gets or sets the name of the member.
    /// </summary>
    /// <value>The name of the member.</value>
    [XmlAttribute("MemberName")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the signatures of the member.
    /// </summary>
    /// <value>The signatures of the member.</value>
    [XmlElement("MemberSignature", Order = 0)]
    public SignatureDefinition[] Signatures { get; set; } = [];

    /// <summary>
    ///     Gets or sets the type of the member.
    /// </summary>
    /// <value>The type of the member.</value>
    [XmlElement("MemberType", Order = 1)]
    public MemberTypes Type { get; set; }
}
