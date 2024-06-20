using Hydrogen.Mapper.Abstraction.Configuration;
using Hydrogen.Mapper.Abstraction.Services;
using System.Linq.Expressions;

namespace Hydrogen.Mapper;
/// <summary>
///     This class provides required services to convert objects and expressions from a source type
///     to a destination type. It's not a standalone mapper and needs a mapping service provider to
///     response mapping requests and converting objects and expressions together. 
/// </summary>
public sealed class Service : IDataMapper, IExpressionMapper
{
    /// <inheritdoc cref="Service"/>
    /// <param name="configuration">
    ///     A configuration object that defined available mapping models. To create a configuration
    ///     object use <see cref="MappingConfigurationBuilder"/>.
    /// </param>
    /// <param name="serviceProvider">
    ///     A mapping service provider that will be used to map expressions and object together.
    /// </param>
    public Service(MappingPlan configuration, IMappingServiceProvider serviceProvider)
    {
        Configuration = configuration;
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    ///     This property contains a mapping configuration object that was passed to the service.
    /// </summary>
    public MappingPlan Configuration { get; } 

    /// <summary>
    ///     This property contains a mapping service provider that was passed to the service.
    /// </summary>
    public IMappingServiceProvider ServiceProvider { get; }

    /// <inheritdoc/>
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return ServiceProvider.GetDataMapper(Configuration)
                              .Map<TSource, TDestination>(source);
    }
    
    /// <inheritdoc/>
    public TDestination Map<TSource, TDestination>(TSource source, MappingPlan configuration)
    {
        return ServiceProvider.GetDataMapper(configuration)
                              .Map<TSource, TDestination>(source);
    }
    
    /// <inheritdoc/>
    public LambdaExpression Map<TSource, TDestination>(LambdaExpression expression)
    {
        return ServiceProvider.GetExpressionMapper(Configuration)
                              .Map<TSource, TDestination>(expression);
    }
    
    /// <inheritdoc/>
    public LambdaExpression Map<TSource, TDestination>(LambdaExpression expression, MappingPlan configuration)
    {
        return ServiceProvider.GetExpressionMapper(configuration)
                              .Map<TSource, TDestination>(expression);
    }
}
