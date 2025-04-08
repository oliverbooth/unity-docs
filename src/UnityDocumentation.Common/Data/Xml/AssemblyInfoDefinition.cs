using System.Xml.Serialization;

namespace UnityDocumentation.Common.Data.Xml;

/// <summary>
///     Denotes which assemblies in which a type is defined.
/// </summary>
public sealed class AssemblyInfoDefinition
{
    /// <summary>
    ///     Gets or sets the name of the assembly in which the type is defined.
    /// </summary>
    /// <value>The name of the assembly in which the type is defined.</value>
    [XmlElement("AssemblyName", Order = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the versions of the assembly in which the type is defined.
    /// </summary>
    /// <value>The versions of the assembly in which the type is defined.</value>
    [XmlElement("AssemblyVersion", Order = 1)]
    public string[] Versions { get; set; } = [];
}
