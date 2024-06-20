namespace Hydrogen.Mapper.Abstraction.Configuration.Enums;

/// <summary>This enum defines different sides of the <see cref="MappingEndpoint"/>.</summary>
public enum EndpointSides : byte
{
    /// <summary>The source of the mapping</summary>
    Source = 1,
    /// <summary>The destination of the mapping</summary>
    Destination = 2
}
