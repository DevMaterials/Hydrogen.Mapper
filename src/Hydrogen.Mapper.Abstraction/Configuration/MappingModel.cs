using Hydrogen.Mapper.Abstraction.Attributes;
using Hydrogen.Mapper.Abstraction.Exceptions;
using Hydrogen.Mapper.Abstraction.Policies;
using System.Reflection;

namespace Hydrogen.Mapper.Abstraction.Configuration;

/// <summary>A mapping model says that how object mappers should map two types together.</summary>
public sealed class MappingModel
{
    private readonly List<MappingRoute> _routes = [];
    private readonly List<IMappingPolicy> _policies = [];

    /// <summary>The type of the source object in the mapping operation.</summary>
    public Type SourceType { get; }

    /// <summary>The type of the destination object in the mapping operation.</summary>
    public Type DestinationType { get; }

    /// <summary>A mapping plan that this model belongs to it.</summary>
    public MappingPlan Plan { get; }

    /// <summary>A list of routes that describe how object mapper should map endpoints.</summary>
    public IReadOnlyCollection<MappingRoute> Routes => _routes.AsReadOnly();

    /// <summary>A list of policies that object mappers should apply them.</summary>
    public IReadOnlyCollection<IMappingPolicy> Policies => _policies.AsReadOnly();

    /// <summary>
    ///     This constructor will be used to to find mapping routes between source and destination
    ///     members by looking for applied mapping attributes on destination members.
    /// </summary>
    /// <param name="plan">See <see cref="Plan"/></param>
    /// <param name="sourceType">See <see cref="SourceType"/></param>
    /// <param name="destinationType">See <see cref="DestinationType"/></param>
    /// <param name="policies">See <see cref="Policies"/></param>
    public MappingModel(MappingPlan plan, 
                        Type sourceType, 
                        Type destinationType, 
                        IEnumerable<IMappingPolicy> policies)
    {
        Plan = plan;
        SourceType = sourceType;
        DestinationType = destinationType;

        _policies.AddRange(policies);

        DiscoverPublicRoutesByAttribute();
    }

    /// <summary>
    ///     This method will be used to define mapping routes between public members of the source
    ///     and destination types by finding those members of the destination type that are marked
    ///     by <see cref="MappedToAttribute"/> attribute. 
    /// </summary>
    private void DiscoverPublicRoutesByAttribute()
    {
        foreach (var destinationMember in DestinationType.GetMembers())
        {
            _routes.AddRange(ProcessMemberByAttribute(destinationMember));
        }
    }

    /// <summary>This method matches a marked destination member with source members.</summary>
    /// <param name="destinationMember">A member of the destination type</param>
    /// <returns>
    ///     Returns a list of mapping routes. For methods and constructor, this method may produce
    ///     more than one route.
    /// </returns>
    /// <exception cref="Hydrogen.Abstraction.Exceptions.NotSupportedException{MemberTypes}" />
    private List<MappingRoute> ProcessMemberByAttribute(MemberInfo destinationMember)
        => destinationMember.MemberType switch
        {
            MemberTypes.Field => ProcessFieldByAttribute(((FieldInfo)destinationMember)),
            MemberTypes.Method => ProcessMethodByAttribute(((MethodInfo)destinationMember)),
            MemberTypes.Property => ProcessPropertyByAttribute(((PropertyInfo)destinationMember)),
            MemberTypes.Constructor => ProcessConstructorByAttribute(((ConstructorInfo)destinationMember)),

            _ => throw new Hydrogen.Abstraction.Exceptions.NotSupportedException<MemberTypes>(
                nameof(destinationMember.MemberType),
                destinationMember.MemberType,
                "The member type is not supported in the mapping operation."),
        };

    /// <summary>This method matches a marked destination field with source members.</summary>
    /// <param name="destinationField">A field of the destination type</param>
    /// <returns>Returns a list of mapping routes with no or one item.</returns>
    private List<MappingRoute> ProcessFieldByAttribute(FieldInfo destinationField)
    {
        var attribute = destinationField.GetCustomAttribute<PairedToAttribute>();

        if (attribute != null)
        {
            var (sourceMember, visitedTypes) = FindSourceMemberByPath(attribute.Path);

            MappingRoute mappingRoute = new(this, sourceMember, destinationField, []);

            return [mappingRoute];
        }

        return [];
    }

    /// <summary>This method matches marked method parameters with source members.</summary>
    /// <param name="destinationMethod">A method of the destination type</param>
    /// <returns>
    ///     Returns a list of mapping routes. For methods, this method may produce no or more than
    ///     one route.
    /// </returns>
    private List<MappingRoute> ProcessMethodByAttribute(MethodInfo destinationMethod)
    {
        List<MappingRoute> discoveredRoutes = [];

        foreach (var parameterInfo in destinationMethod.GetParameters())
        {
            var attribute = parameterInfo.GetCustomAttribute<PairedToAttribute>();

            if (attribute != null)
            {
                var (sourceMember, visitedTypes) = FindSourceMemberByPath(attribute.Path);

                MappingRoute mappingRoute = new(this, sourceMember, destinationMethod, parameterInfo, []);

                discoveredRoutes.Add(mappingRoute);
            }
        }

        return discoveredRoutes;
    }

    /// <summary>This method matches a marked destination property with source members.</summary>
    /// <param name="destinationProperty">A property of the destination type</param>
    /// <returns>Returns a list of mapping routes with no or one item.</returns>
    private List<MappingRoute> ProcessPropertyByAttribute(PropertyInfo destinationProperty)

    {
        List<MappingRoute> discoveredRoutes = [];

        var attribute = destinationProperty.GetCustomAttribute<PairedToAttribute>();

        if (attribute != null)
        {
            var (sourceMember, visitedTypes) = FindSourceMemberByPath(attribute.Path);

            MappingRoute mappingRoute = new(this, sourceMember, destinationProperty, []);

            discoveredRoutes.Add(mappingRoute);
        }

        return discoveredRoutes;
    }

    /// <summary>This method matches marked constructor parameters with source members.</summary>
    /// <param name="destinationConstructor">A constructor of the destination type</param>
    /// <returns>
    ///     Returns a list of mapping routes. For constructor, this method may produce no or more 
    ///     than one route.
    /// </returns>
    private List<MappingRoute> ProcessConstructorByAttribute(ConstructorInfo destinationConstructor)
    {
        List<MappingRoute> discoveredRoutes = [];

        foreach (var parameterInfo in destinationConstructor.GetParameters())
        {
            var attribute = parameterInfo.GetCustomAttribute<PairedToAttribute>();

            if (attribute != null)
            {
                var (sourceMember, visitedTypes) = FindSourceMemberByPath(attribute.Path);

                MappingRoute mappingRoute = new(this, sourceMember, destinationConstructor, parameterInfo, []);

                discoveredRoutes.Add(mappingRoute);
            }
        }

        return discoveredRoutes;
    }

    /// <summary>
    ///     This method discovers a source member that was specified by the destination as a source
    ///     member for the mapping. 
    /// </summary>
    /// <param name="path">See <see cref="PairedToAttribute.Path"/></param>
    /// <returns>
    ///     This method returns a tuple with two members. The first member returns the member info 
    ///     of the specified source member and second one returns a list of types that were placed 
    ///     between the source and destination types in this route. 
    /// </returns>
    /// <exception cref="InvalidBindingPathException"></exception>
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
                throw new InvalidBindingPathException(SourceType, DestinationType, $"{traversed}{memberName}");
            }

            traversed += $"{memberName}.";
            sourceMember = sourceMembers.First();
            currentType = MappingEndpoint.GetMemberValueType(sourceMember);
        }

        if (traversed.TrimEnd('.') != path || sourceMember == null)
        {
            throw new Exceptions.InvalidBindingPathException(SourceType, DestinationType, $"{traversed}");
        }

        return (sourceMember, visitedTypes);
    }
}
