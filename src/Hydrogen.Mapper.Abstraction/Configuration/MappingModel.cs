using Hydrogen.Mapper.Abstraction.Configuration.Attributes;
using Hydrogen.Mapper.Abstraction.Exceptions;
using System.Reflection;
using System.Text.Json;

namespace Hydrogen.Mapper.Abstraction.Configuration;

public class MappingModel
{
    public MappingModel(MappingConfiguration configuration, Type sourceType, Type destinationType)
    {
        Configuration = configuration;
        SourceType = sourceType;
        DestinationType = destinationType;
        Routes = DiscoverRoutesByAttribute();
    }


    public Type SourceType { get; }
    public Type DestinationType { get; }
    public MappingConfiguration Configuration { get; }
    public IReadOnlyCollection<MappingRoute> Routes { get; }


    private List<MappingRoute> DiscoverRoutesByAttribute()
    {
        List<MappingRoute> routes = [];

        var destinationMembers = DestinationType.GetMembers();

        foreach (var destinationMember in destinationMembers)
        {
            routes.AddRange(ProcessMember(destinationMember));
        }

        return routes;
    }
    private IEnumerable<MappingRoute> ProcessMember(MemberInfo destinationMember)
        => destinationMember.MemberType switch
        {
            MemberTypes.Field => ProcessField(((FieldInfo)destinationMember)),
            MemberTypes.Method => ProcessMethod(((MethodInfo)destinationMember)),
            MemberTypes.Property => ProcessProperty(((PropertyInfo)destinationMember)),
            MemberTypes.Constructor => ProcessConstructor(((ConstructorInfo)destinationMember)),

            _ => throw new InvalidMappingTypesException(SourceType.Name, DestinationType.Name),
        };
    private IEnumerable<MappingRoute> ProcessField(FieldInfo fieldInfo)
    {
        List<MappingRoute> discoveredRoutes = [];

        var attribute = fieldInfo.GetCustomAttribute<PairedToAttribute>();

        if (attribute != null)
        {
            var (sourceMember, visitedTypes) = FindSourceMemberByPath(attribute.Path);

            MappingRoute mappingRoute = new(this, sourceMember, fieldInfo);

            discoveredRoutes.Add(mappingRoute);
        }

        return discoveredRoutes;
    }
    private IEnumerable<MappingRoute> ProcessMethod(MethodInfo methodInfo)
    {
        List<MappingRoute> discoveredRoutes = [];

        foreach (var parameterInfo in methodInfo.GetParameters())
        {
            var attribute = parameterInfo.GetCustomAttribute<PairedToAttribute>();

            if (attribute != null)
            {
                var (sourceMember, visitedTypes) = FindSourceMemberByPath(attribute.Path);

                MappingRoute mappingRoute = new(this, sourceMember, methodInfo, parameterInfo);

                discoveredRoutes.Add(mappingRoute);
            }
        }

        return discoveredRoutes;
    }
    private IEnumerable<MappingRoute> ProcessProperty(PropertyInfo propertyInfo)

    {
        List<MappingRoute> discoveredRoutes = [];

        var attribute = propertyInfo.GetCustomAttribute<PairedToAttribute>();

        if (attribute != null)
        {
            var (sourceMember, visitedTypes) = FindSourceMemberByPath(attribute.Path);

            MappingRoute mappingRoute = new(this, sourceMember, propertyInfo);

            discoveredRoutes.Add(mappingRoute);
        }

        return discoveredRoutes;
    }
    private IEnumerable<MappingRoute> ProcessConstructor(ConstructorInfo constructorInfo)
    {
        List<MappingRoute> discoveredRoutes = [];

        foreach (var parameterInfo in constructorInfo.GetParameters())
        {
            var attribute = parameterInfo.GetCustomAttribute<PairedToAttribute>();

            if (attribute != null)
            {
                var (sourceMember, visitedTypes) = FindSourceMemberByPath(attribute.Path);

                MappingRoute mappingRoute = new(this, sourceMember, constructorInfo, parameterInfo);

                discoveredRoutes.Add(mappingRoute);
            }
        }

        return discoveredRoutes;
    }
   

    private (MemberInfo, IEnumerable<Type>) FindSourceMemberByPath(string path)
    {
        var memberNames = path.Split('.');
        var currentType = SourceType;
        var traversed = string.Empty;
        MemberInfo? sourceMember = null;
        List<Type> visitedTypes = [currentType];

        foreach (string memberName in memberNames)
        {
            var sourceMembers = currentType.GetMember(memberName);

            if (sourceMembers == null || sourceMembers.Length == 0)
            {
                throw new InvalidBindingPathException($"{traversed}{memberName}");
            }

            traversed += $"{memberName}.";
            sourceMember = sourceMembers.First();
            currentType = MappingEndpoint.GetMemberValueType(sourceMember);
        }

        if (traversed.TrimEnd('.') != path || sourceMember == null)
        {
            throw new InvalidBindingPathException($"{traversed}");
        }

        return (sourceMember, visitedTypes);
    }
}
