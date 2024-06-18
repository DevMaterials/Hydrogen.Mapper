namespace Hydrogen.Mapper.Abstraction.Configuration.Attributes;

/// <summary>
///     This attribute specifies that how object mapper should map a marked member or parameter to 
///     to another member of a class or struct that its path was specified. 
/// </summary>
/// <param name="path">
///     A dot-separated path of the source member from the mapping source type. 
/// </param>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
public class PairedToAttribute(string path) : Attribute
{
    /// <summary>
    ///     This property holds a dot-separated path of the source member from the mapping source type. 
    /// </summary>
    public string Path { get; } = path;
}