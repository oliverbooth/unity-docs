using System.Xml.Serialization;

namespace UnityDocumentation.Common.Data.Xml;

/// <summary>
///     Contains the members of a type.
/// </summary>
public sealed class MembersDefinition
{
    /// <summary>
    ///     Gets or sets the members of the type.
    /// </summary>
    /// <value>The members of the type.</value>
    [XmlElement("Member")]
    public MemberDefinition[] Members { get; set; } = [];
}