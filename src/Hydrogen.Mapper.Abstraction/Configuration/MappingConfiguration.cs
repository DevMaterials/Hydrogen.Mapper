using Hydrogen.Mapper.Abstraction.Configuration.Attributes;
using System.Reflection;
using System.Text.Json;

namespace Hydrogen.Mapper.Abstraction.Configuration;

/// <summary>
///     An object mapper needs a plan to map different objects together. This class will be used to
///     define a mapping configuration for converting different types and their members. 
/// </summary>
public sealed class MappingConfiguration
{
    /// <summary>
    ///     This constructor creates a configuration object by discovering models automatically. In 
    ///     this approach, the modeler tries to discover those types that they are mapped to others
    ///     by matching marker attributes or by the similarity of the types' structures. 
    /// </summary>
    /// <param name="assemblies">
    ///     A list of assemblies that are available in the application context.
    /// </param>
    public MappingConfiguration(IEnumerable<Assembly> assemblies)
    {
        _models.AddRange(DiscoverModelsByAttribute(assemblies).AsReadOnly<MappingModel>());
    }

    /// <summary>
    ///     This constructor creates a configuration object by discovering predefined models in the
    ///     JSON config file that its path will be passed to the constructor.
    /// </summary>
    /// <param name="assemblies">
    ///     A list of assemblies that are available in the application context.
    /// </param>
    /// <param name="jsonFilePath">The full path of the JSON config file.</param>
    public MappingConfiguration(IEnumerable<Assembly> assemblies, string jsonFilePath)
    {
        //TODO: This feature is not implemented yet.
        throw new NotImplementedException();
    }

    /// <summary>
    ///     This constructor creates a configuration object by discovering predefined models in the
    ///     JSON document that it will be passed to the constructor.
    /// </summary>
    /// <param name="assemblies">
    ///     A list of assemblies that are available in the application context.
    /// </param>    
    /// <param name="jsonDocument">A JSON document that contains predefined mapping models.</param>
    public MappingConfiguration(IEnumerable<Assembly> assemblies, JsonDocument jsonDocument)
    {
        //TODO: This feature is not implemented yet.
        throw new NotImplementedException();
    }

    private readonly List<MappingModel> _models = [];

    /// <summary>
    ///     A list of discovered mapping models.
    /// </summary>
    public IReadOnlyCollection<MappingModel> Models => _models.AsReadOnly();

    /// <summary>
    ///     This method will be used to discover mapping models by using mapping marker attributes.
    /// </summary>
    /// <param name="assemblies">
    ///     A list of assemblies that are available in the application context.
    /// </param>    
    /// <returns>
    ///     This method returns a list of discovered mapping models that might be empty.
    /// </returns>
    private List<MappingModel> DiscoverModelsByAttribute(IEnumerable<Assembly> assemblies)
    {
        List<MappingModel> discoveredModels = [];

        var allTypes = assemblies.SelectMany(assembly => assembly.GetTypes());

        foreach (var destinationType in allTypes)
        {
            var attribute = destinationType.GetCustomAttribute<MappedToAttribute>();

            if (attribute != null)
            {
                discoveredModels.Add(new MappingModel(this, attribute.SourceType, destinationType));
            }
        }

        return discoveredModels;
    }

    internal MappingModel DefineModel(Type sourceType, Type destinationType)
    {
        var model = _models.FirstOrDefault(m => m.SourceType.Equals(sourceType) && m.DestinationType.Equals(destinationType));

        if (model == null)
        {
            model = new(this, sourceType, destinationType);
            _models.Add(model);
        }
        return model;
    }
}
