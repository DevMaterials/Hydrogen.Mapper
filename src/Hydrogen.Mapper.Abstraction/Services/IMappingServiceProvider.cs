using Hydrogen.Mapper.Abstraction.Configuration;

namespace Hydrogen.Mapper.Abstraction.Services;

/// <summary>
///     This contract will be used to define required attributes and functionalities of expressions
///     and data mappers.Hydrogen Mapper is not a standalone mapper service and needs other mapping
///     service providers to map objects and expressions together. So, each service provider should
///     Implement this interface to expose its data and expression mapper services. 
/// </summary>
public interface IMappingServiceProvider
{
    /// <summary>This method will be used to get an instance of the data mapper.</summary>
    /// <param name="plan">See <see cref="MappingPlan"/></param>
    /// <returns>Returns an instance of the data mapper.</returns>
    IDataMapper GetDataMapper(MappingPlan plan);

    /// <summary>This method will be used to get an instance of the expression mapper.</summary>
    /// <param name="plan">See <see cref="MappingPlan"/></param>
    /// <returns>Returns an instance of the expression mapper.</returns>
    IExpressionMapper GetExpressionMapper(MappingPlan plan);
}
