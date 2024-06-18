namespace Hydrogen.Mapper.Abstraction.Configuration.Attributes;

/// <summary>
///     This marker attribute links a marked class or struct as the mapping destination to another
///     class or struct as a source of the mapping operation.
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

