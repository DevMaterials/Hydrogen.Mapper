using AutoMapper;
using Hydrogen.Mapper.Abstraction.Configuration;
using Hydrogen.Mapper.Abstraction.Services;

namespace Hydrogen.Mapper.Extensions.AutoMapper;

internal class AutoDataMapper : IDataMapper
{
    private MapperConfiguration _autoMapperConfiguration;

    public AutoDataMapper(MappingPlan plan)
    {
        MappingConfigurationBuilder autoMapperConfigBuilder = new(plan);
        _autoMapperConfiguration = autoMapperConfigBuilder.Build();
    }

    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return _autoMapperConfiguration.CreateMapper().Map<TSource, TDestination>(source);
    }

    public TDestination Map<TSource, TDestination>(TSource source, MappingPlan plan)
    {
        var autoMapperConfiguration = new MappingConfigurationBuilder(plan).Build();

        return autoMapperConfiguration.CreateMapper().Map<TSource, TDestination>(source);
    }
}
