namespace Hydrogen.Mapper.Abstraction.Configuration.Enums;

/// <summary>This enum defines different types of the <see cref="MappingRoute"/>.</summary>
public enum RouteTypes : byte
{
    /// <summary>Destination endpoint is directly mapped to the source.</summary>
    Direct = 1,

    /// <summary>The destination type is simplified compared to the source type.</summary>
    FlattenedRoute = 2,

    /// <summary>The source type is simplified compared to the destination type.</summary>
    UnflattenedRoute = 3,

    /// <summary>Source and destination endpoints are not directly connected together.</summary>
    FullyUnflattenedRoute = 4,
}
