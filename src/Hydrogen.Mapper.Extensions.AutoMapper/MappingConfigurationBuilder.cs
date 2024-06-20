using AutoMapper;
using Hydrogen.Mapper.Abstraction.Configuration;

namespace Hydrogen.Mapper.Extensions.AutoMapper;

public class MappingConfigurationBuilder(MappingPlan plan)
{
    private readonly MappingPlan _plan = plan;

    public MapperConfiguration Build()
    {
        MapperConfigurationExpression expression = new();
        return new MapperConfiguration(expression);
    }
}
