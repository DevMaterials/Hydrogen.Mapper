using Hydrogen.Mapper.Abstraction.Configuration;

namespace Hydrogen.Mapper.Abstraction.Attributes;

/// <summary>
///     This attribute defines a relationship between a destination <see cref="MappingEndpoint"/> 
///     and a source <see cref="MappingEndpoint"/> of the mapping operation. The attribute will be
///     applied on the destination of the operation and the source will be specified by the passed
///     parameter. You can only mark fields, properties,or parameters of constructors or methods by
///     this attribute. 
/// </summary>
/// <param name="path">
///     A dot-separated path of the source member from the mapping source type as the origin. 
/// </param>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
public class PairedToAttribute(string path) : Attribute
{
    /// <summary>
    ///     This property contains a dot-separated path of the source member from the source type.
    /// </summary>
    public string Path { get; } = path;
}