using Hydrogen.Abstraction.Exceptions;
using Hydrogen.Mapper.Abstraction.Configuration.Enums;
using System.Reflection;

namespace Hydrogen.Mapper.Abstraction.Configuration;

/// <summary>
///     Each mapping operation has two sides; A source which is an existing object with data, and a
///     target object that should be created and filled by the source data. A mapping route creates 
///     an endpoint for each field, property, method and constructor were involved in the mapping.
/// </summary>
public sealed class MappingEndpoint
{
    /// <summary>
    ///     A name for the endpoint that can be the name of the member or the name of the parameter
    ///     of the member. 
    /// </summary>
    public string EndpointName { get; }

    /// <summary>This property contains the member's value type.</summary>
    public Type ValueType { get; }

    /// <summary>See <see cref="EndpointTypes"/></summary>
    public EndpointTypes EndpointType { get; }

    /// <summary>See <see cref="EndpointSides"/></summary>
    public EndpointSides EndpointSide { get; }

    /// <summary>
    ///     This constructor will be used to create an endpoint for fields or properties. 
    /// </summary>
    /// <param name="member">A member information</param>
    /// <param name="side">Indicates that it is a source endpoint or destination endpoint.</param>
    public MappingEndpoint(MemberInfo member, EndpointSides side)
    {
        EndpointName = member.Name;
        ValueType = GetMemberValueType(member);
        EndpointType = SetEndpointType(member);
        EndpointSide = side;
    }

    /// <summary>
    ///     This constructor will be used to create an endpoint for methods and constructors.
    /// </summary>
    /// <param name="member">A member information</param>
    /// <param name="parameter">The parameter of the method or constructor</param>
    /// <param name="side">Indicates that it is a source endpoint or destination endpoint.</param>
    public MappingEndpoint(MemberInfo member, ParameterInfo parameter, EndpointSides side)
    {
        EndpointName = $"{member.Name}.{parameter.Name}";
        ValueType = GetParameterValueTypeName(parameter);
        EndpointType = SetEndpointType(member);
        EndpointSide = side;

    }

    /// <summary>
    ///     This method returns the type of the endpoint depending on the member's type.
    /// </summary>
    /// <param name="member"></param>
    /// <returns>Returns the type of the endpoint</returns>
    /// <exception cref="NotSupportedException{MemberTypes}"></exception>
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

    /// <summary>This method returns the value type of a constructor or method parameter.</summary>
    /// <param name="parameterInfo">A parameter information</param>
    /// <returns>Returns the type of the value that parameter holds.</returns>
    private Type GetParameterValueTypeName(ParameterInfo parameterInfo)
    {
        return parameterInfo.ParameterType;
    }

    /// <summary>
    ///     This method will be used to return the value type of a member. 
    /// </summary>
    /// <param name="member">The member information that we want to get its value type</param>
    /// <returns>The value type of the member</returns>
    /// <exception cref="NotSupportedException{MemberTypes}"></exception>
    internal static Type GetMemberValueType(MemberInfo member)
    {
        return member.MemberType switch
        {
            MemberTypes.Field => ((FieldInfo)member).FieldType,
            MemberTypes.Property => ((PropertyInfo)member).PropertyType,

            _ => throw new NotSupportedException<MemberTypes>(
                           nameof(member.MemberType),
                           member.MemberType,
                           "The type of the member is not appropriate for mapping operation.")
        };
    }
}
