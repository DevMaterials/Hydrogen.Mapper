using Hydrogen.Mapper.Abstraction.Attributes;
using Hydrogen.Mapper.Abstraction.Policies;
using System.Reflection;
using System.Text.Json;

namespace Hydrogen.Mapper.Abstraction.Configuration;

/// <summary>
///     An object mapper requires a plan that it says how a mapper should convert different objects
///     together. This class will be used to define that plan.
/// </summary>
public sealed class MappingPlan
{
    private readonly List<MappingModel> _models = [];
    private readonly List<IMappingPolicy> _generalPolicies = [];

    /// <summary>This property returns a read only list of the discovered mapping models.</summary>
    public IReadOnlyCollection<MappingModel> Models => _models.AsReadOnly();
    
    /// <summary>A read only list of general policies that should be applied by mappers.</summary>
    public IReadOnlyCollection<IMappingPolicy> GeneralPolicies => _generalPolicies.AsReadOnly();

    /// <summary>
    ///     This constructor creates a mapping plan by discovering models automatically and finding
    ///     those types that are mapped to others by marker attributes or have similar structures.
    /// </summary>
    /// <param name="assemblies">Assemblies that are available in the application context.</param>
    /// <param name="generalPolicies">See <see cref="GeneralPolicies"/></param>
    public MappingPlan(IEnumerable<Assembly> assemblies, IEnumerable<IMappingPolicy> generalPolicies)
    {
        _generalPolicies.AddRange(generalPolicies); 

        DiscoverModelsByAttribute(assemblies);
    }

    /// <summary>This constructor creates a mapping plan by using a json object.</summary>
    /// <param name="assemblies">Assemblies that are available in the application context.</param>
    /// <param name="jsonDocument">A JSON document that contains predefined mapping models.</param>
    public MappingPlan(IEnumerable<Assembly> assemblies, JsonDocument jsonDocument)
    {
        //TODO: This feature is not implemented yet.
        throw new NotImplementedException();
    }

    /// <summary>
    ///     This method finds mapping types that were marked by <see cref="MappedToAttribute"/> and
    ///     defines relevant mapping models for them. 
    /// </summary>
    /// <param name="assemblies">Assemblies that are available in the application context.</param>
    private void DiscoverModelsByAttribute(IEnumerable<Assembly> assemblies)
    {
        foreach (var destinationType in assemblies.SelectMany(assembly => assembly.GetTypes()))
        {
            var attribute = destinationType.GetCustomAttribute<MappedToAttribute>();

            if (attribute != null)
            {
                _models.Add(new MappingModel(this, attribute.SourceType, destinationType, []));
            }
        }
    }
}
