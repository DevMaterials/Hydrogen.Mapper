using Hydrogen.Mapper.Abstraction.Configuration;
using Hydrogen.Mapper.Abstraction.Services;

namespace Hydrogen.Mapper.Extensions.AutoMapper;

public class AutoMapperService : IMappingServiceProvider
{
    public IDataMapper GetDataMapper(MappingPlan plan)
    {
        AutoDataMapper dataMapper = new(plan);

        return dataMapper;
    }
    public IExpressionMapper GetExpressionMapper(MappingPlan plan)
    {
        AutoExpressionMapper expressionMapper = new(plan);

        return expressionMapper;
    }
}
