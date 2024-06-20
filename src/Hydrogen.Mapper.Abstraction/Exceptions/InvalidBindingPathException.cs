using Hydrogen.Abstraction.Exceptions;
using Hydrogen.Mapper.Abstraction.Attributes;

namespace Hydrogen.Mapper.Abstraction.Exceptions;

/// <summary>
///     This exception will be raised when the <see cref="PairedToAttribute.Path"/> is not valid. 
/// </summary>
/// <param name="sourceType">See <see cref="SourceType"/></param>
/// <param name="destinationType">See <see cref="DestinationType"/></param>
/// <param name="path">See <see cref="Path"/></param>
public sealed class InvalidBindingPathException(Type sourceType, Type destinationType, string path) : AbstractException
{
    /// <summary>The source type of the mapping.</summary>
    public Type SourceType { get; } = sourceType;
   
    /// <summary>The destination type of the mapping.</summary>
    public Type DestinationType { get; } = destinationType;
   
    /// <summary>The path that was specified by <see cref="PairedToAttribute.Path"/></summary>
    public string Path { get; } = path;
}
