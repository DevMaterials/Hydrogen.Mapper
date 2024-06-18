using Hydrogen.Mapper.Abstraction.Enums;
using System.Reflection;

namespace Hydrogen.Mapper.Abstraction.Configuration;

public class MappingRoute
{
    public MappingRoute(MappingModel model,
                        MemberInfo sourceMember,
                        MemberInfo destinationMember)
    {
        Model = model;
        Type = SetRouteType(sourceMember, destinationMember);
        Source = new(sourceMember, EndpointSides.Source);
        Destination = new(destinationMember, EndpointSides.Destination);
    }

    public MappingRoute(MappingModel model,
                        MemberInfo sourceMember,
                        MemberInfo destinationMember,
                        ParameterInfo destinationParameter)
    {
        Model = model;
        Type = SetRouteType(sourceMember, destinationMember);
        Source = new(sourceMember, EndpointSides.Source);
        Destination = new(destinationMember, destinationParameter, EndpointSides.Destination);
    }

    public RouteTypes Type { get; }
    public MappingModel Model { get; }
    public MappingEndpoint Source { get; }
    public MappingEndpoint Destination { get; }

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
