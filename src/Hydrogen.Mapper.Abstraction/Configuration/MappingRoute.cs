using Hydrogen.Mapper.Abstraction.Configuration.Enums;
using Hydrogen.Mapper.Abstraction.Policies;
using System.Reflection;

namespace Hydrogen.Mapper.Abstraction.Configuration;

/// <summary>
///     A mapping route defines a relationship between two mapping endpoints. It contains required
///     information about how object mapper should map a destination member to the relevant source 
///     member during the mapping operation.
/// </summary>
public class MappingRoute
{
    private readonly List<IMappingPolicy> _policies = [];

    /// <summary>See <see cref="RouteTypes"/></summary>
    public RouteTypes Type { get; }
   
    /// <summary>This property indicates that which mapping model this route belongs to.</summary>
    public MappingModel Model { get; }
    
    /// <summary>An endpoint that was created by the source member information.</summary>
    public MappingEndpoint Source { get; }

    /// <summary>An endpoint that was created by the destination member information.</summary>
    public MappingEndpoint Destination { get; }

    /// <summary>A list of policies that object mapper should apply them on the mapping.</summary>
    public IReadOnlyCollection<IMappingPolicy> Policies => _policies.AsReadOnly();

    /// <summary>
    ///     This constructor takes two members information and creates required endpoints and other
    ///     route's information depending on them. 
    /// </summary>
    /// <param name="model">See <see cref="Model"/></param>
    /// <param name="sourceMember">The source member information.</param>
    /// <param name="destinationMember">The destination member information</param>
    /// <param name="policies">See <see cref="Policies"/></param>
    public MappingRoute(MappingModel model,
                        MemberInfo sourceMember,
                        MemberInfo destinationMember,
                        IEnumerable<IMappingPolicy> policies)
    {
        Model = model;
        Type = SetRouteType(sourceMember, destinationMember);
        Source = new(sourceMember, EndpointSides.Source);
        Destination = new(destinationMember, EndpointSides.Destination);

        _policies.AddRange(policies);
    }

    /// <summary>
    ///     This constructor takes two members information and binds a parameter of the destination 
    ///     method or constructor to the source member. 
    /// </summary>
    /// <param name="model">See <see cref="Model"/></param>
    /// <param name="sourceMember">The source member information.</param>
    /// <param name="destinationMember">The destination method or constructor information.</param>
    /// <param name="destinationParameter">A parameter of the method or constructor.</param>
    /// <param name="policies">See <see cref="Policies"/></param>
    public MappingRoute(MappingModel model,
                        MemberInfo sourceMember,
                        MemberInfo destinationMember,
                        ParameterInfo destinationParameter,
                        IEnumerable<IMappingPolicy> policies)
    {
        Model = model;
        Type = SetRouteType(sourceMember, destinationMember);
        Source = new(sourceMember, EndpointSides.Source);
        Destination = new(destinationMember, destinationParameter, EndpointSides.Destination);
     
        _policies.AddRange(policies);
    }

    /// <summary>
    ///     This method takes member's information and returns the type of a route between them.
    /// </summary>
    /// <param name="sourceMember">The source member information</param>
    /// <param name="destinationMember">The destination member information</param>
    /// <returns>A relevant route type</returns>
    private RouteTypes SetRouteType(MemberInfo sourceMember, MemberInfo destinationMember)
    {
        if (Model.SourceType == sourceMember.DeclaringType)
        {
            return Model.DestinationType == destinationMember.DeclaringType ? RouteTypes.Direct
                                                                            : RouteTypes.UnflattenedRoute;
        }
        else if (Model.DestinationType == destinationMember.DeclaringType)
        {
            return RouteTypes.FlattenedRoute;
        }

        return RouteTypes.FullyUnflattenedRoute;
    }
}
