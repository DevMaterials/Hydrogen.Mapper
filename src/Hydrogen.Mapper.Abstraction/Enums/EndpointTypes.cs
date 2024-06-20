namespace Hydrogen.Mapper.Abstraction.Configuration.Enums;

/// <summary>This enum defines different types of the <see cref="MappingEndpoint"/>.</summary>
public enum EndpointTypes : byte
{
    /// <summary>A field of a class or struct.</summary>
    Field = 1,

    /// <summary>A property of a class or struct.</summary>
    Property = 2,

    /// <summary>A parameter of a method of a class or a struct.</summary>
    MethodParameter = 3,

    /// <summary>A parameter of a constructor of a class or a struct.</summary>
    ConstructorParameter = 4,
}
