using Hydrogen.Mapper.Abstraction.Configuration;

namespace Hydrogen.Mapper.Abstraction.Services;

/// <summary>This contract defines attributes and functionalities of data mappers.</summary>
public interface IDataMapper
{
    /// <summary>This method makes an object and initializes it by another object's data.</summary>
    /// <typeparam name="TSource">The type of the source object</typeparam>
    /// <typeparam name="TDestination">The type of the target object</typeparam>
    /// <param name="source">The source object that its data will be used in the mapping.</param>
    /// <returns>Returns a mapped object as the type of the mapping destination type.</returns>
    TDestination Map<TSource, TDestination>(TSource source);

    /// <summary>This method makes an object and initializes it by another object's data.</summary>
    /// <typeparam name="TSource">The type of the source object</typeparam>
    /// <typeparam name="TDestination">The type of the target object</typeparam>
    /// <param name="source">The source object that its data will be used in the mapping.</param>
    /// <param name="customPlan">A plan that will be used instead of the default plan.</param>
    /// <returns>Returns a mapped object as the type of the mapping destination type.</returns>
    TDestination Map<TSource, TDestination>(TSource source, MappingPlan customPlan);
}
