using Hydrogen.Abstraction.Enums;
using Hydrogen.Abstraction.Exceptions;
using Hydrogen.Abstraction.Helpers.Strings;
using Hydrogen.Mapper.Abstraction.Enums;
using System.Reflection;

namespace Hydrogen.Mapper.Abstraction.Configuration;

public sealed class MappingEndpoint
{
    public MappingEndpoint(MemberInfo member, EndpointSides side)
    {
        EndpointName = member.Name;
        ValueType = GetMemberValueTypeName(member);
        EndpointType = SetEndpointType(member);
        EndpointSide = side;
    }

    public MappingEndpoint(MemberInfo member, ParameterInfo parameter, EndpointSides side)
    {
        EndpointName = $"{member.Name}.{parameter.Name}";
        ValueType = GetParameterValueTypeName(parameter);
        EndpointType = SetEndpointType(member);
        EndpointSide = side;

    }

    public string EndpointName { get; }
    public string ValueType { get; }
    public EndpointTypes EndpointType { get; }
    public EndpointSides EndpointSide { get; }
    public IEnumerable<NamingConventions> CompatibleConventions => EndpointName.DetectConventions();

    private static EndpointTypes SetEndpointType(MemberInfo member)
    {
        return member.MemberType switch
        {
            MemberTypes.Field => EndpointTypes.Field,
            MemberTypes.Property => EndpointTypes.Property,
            MemberTypes.Method => EndpointTypes.MethodParameter,
            MemberTypes.Constructor => EndpointTypes.ConstructorParameter,

            _ => throw new NotSupportedException<MemberTypes>(
                           nameof(member.MemberType),
                           member.MemberType,
                           "The type of the member is not appropriate for mapping operation.")
        };
    }

    private  string GetMemberValueTypeName(MemberInfo member)
    {
        return member.MemberType switch
        {
            MemberTypes.Field => ((FieldInfo)member).FieldType.Name,
            MemberTypes.Property => ((PropertyInfo)member).PropertyType.Name,

            //TODO: We should decide about these two types
            MemberTypes.Method => throw new NotImplementedException(),
            MemberTypes.Constructor => throw new NotImplementedException(),

            _ => throw new NotSupportedException<MemberTypes>(
                           nameof(member.MemberType),
                           member.MemberType,
                           "The type of the member is not appropriate for mapping operation.")
        };
    }

    private  string GetParameterValueTypeName(ParameterInfo parameterInfo)
    {
        return parameterInfo.ParameterType.Name;
    }

    internal static Type GetMemberValueType(MemberInfo member)
    {
        return member.MemberType switch
        {
            MemberTypes.Field => ((FieldInfo)member).FieldType,
            MemberTypes.Property => ((PropertyInfo)member).PropertyType,

            //TODO: We should decide about these two types
            MemberTypes.Method => throw new NotImplementedException(),
            MemberTypes.Constructor => throw new NotImplementedException(),

            _ => throw new NotSupportedException<MemberTypes>(
                           nameof(member.MemberType),
                           member.MemberType,
                           "The type of the member is not appropriate for mapping operation.")
        };
    }
}
