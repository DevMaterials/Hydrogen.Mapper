using AutoMapper;
using Hydrogen.Mapper.Abstraction.Configuration;
using Hydrogen.Mapper.Abstraction.Services;
using System.Linq.Expressions;

namespace Hydrogen.Mapper.Extensions.AutoMapper;

public class AutoExpressionMapper : IExpressionMapper
{
    private MapperConfiguration _autoMapperConfiguration;

    public AutoExpressionMapper(MappingPlan plan)
    {
        MappingConfigurationBuilder autoMapperConfigBuilder = new(plan);
        _autoMapperConfiguration = autoMapperConfigBuilder.Build();
    }

    public LambdaExpression Map<TSource, TDestination>(LambdaExpression expression)
    {
        throw new NotImplementedException();
    }

    public LambdaExpression Map<TSource, TDestination>(LambdaExpression expression, MappingPlan configuration)
    {
        throw new NotImplementedException();
    }
}
