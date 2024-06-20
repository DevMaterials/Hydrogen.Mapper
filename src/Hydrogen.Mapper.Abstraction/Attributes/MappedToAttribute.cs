namespace Hydrogen.Mapper.Abstraction.Attributes;

/// <summary>
///     This attribute will be used to map a class or struct as the destination,to another class or
///     struct as the source of a mapping operation. The attribute is used by a destination type of
///     the mapping operation and the source will be specified by the passed parameter. 
/// </summary>
/// <param name="sourceType">The type of the source of mapping operation.</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class MappedToAttribute(Type sourceType) : Attribute
{
    /// <summary>
    ///     This property contains a type that is the source of the mapping operation. 
    /// </summary>
    public Type SourceType { get; } = sourceType;
}