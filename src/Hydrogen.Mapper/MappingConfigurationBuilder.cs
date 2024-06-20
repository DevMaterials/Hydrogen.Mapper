using Hydrogen.Abstraction.Exceptions;
using Hydrogen.Mapper.Abstraction.Configuration;
using Hydrogen.Mapper.Exceptions;
using System.Reflection;
using System.Text.Json;

namespace Hydrogen.Mapper;

/// <summary>
///     This class will be used to make a mapping configuration object from types and models of the
///     registered assemblies. 
/// </summary>
public class MappingConfigurationBuilder
{
    private JsonDocument? _jsonConfiguration = null;
    private readonly List<Assembly> _registeredAssemblies = [];

    /// <summary>
    ///     This method will be used to register an assembly in the list of the assemblies.  
    /// </summary>
    /// <param name="assembly">An assembly that should be registered in the list.</param>
    /// <returns>Returns the current mapping configuration builder object</returns>
    public MappingConfigurationBuilder Register(Assembly assembly)
    {
        if (_registeredAssemblies.Contains(assembly) == false)
        {
            _registeredAssemblies.Add(assembly);
        }
        
        return this;
    }

    /// <summary>
    ///     This method will be used to register multiple assemblies in the list of the assemblies.
    /// </summary>
    /// <param name="assemblies">
    ///     A list of the assemblies that should be registered in the list.
    /// </param>
    /// <returns>Returns the current mapping configuration builder object</returns>
    public MappingConfigurationBuilder Register(Assembly[] assemblies)
    {
        foreach(Assembly assembly in assemblies)
        {
            Register(assembly);
        }

        return this;
    }

    /// <summary>
    ///     This method accepts a string of the Json format and convert it to the JsonDocument type
    ///     for creation of a mapping configuration object. 
    /// </summary>
    /// <param name="configuration">
    ///     A predefined mapping configuration in the Json format. 
    /// </param>
    /// <returns>Returns the current mapping configuration builder object</returns>
    /// <exception cref="InvalidJsonConfigurationException" />
    public MappingConfigurationBuilder LoadConfiguration(string configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration))
        {
            throw new InvalidParameterException(nameof(configuration), configuration, "The json configuration can't be null or empty string.");
        }

        try
        {
            _jsonConfiguration = JsonDocument.Parse(configuration);
        }
        catch (Exception)
        {
            throw new InvalidParameterException(nameof(configuration), configuration, "Invalid json configuration was provided.");
        }

        return this;
    }

    /// <summary>
    ///     This method will be used to load a predefined mapping configuration file and convert it
    ///     to the JsonDocument type. 
    /// </summary>
    /// <param name="path">The full path of the Json file.</param>
    /// <returns>Returns the current mapping configuration builder object</returns>
    /// <exception cref="InvaidFileException" />
    /// <exception cref="InvalidJsonConfigurationException" />
    public MappingConfigurationBuilder LoadFile(string path)
    {
        if(System.IO.File.Exists(path) == false)
        {
            throw new InvaidFileException(path, "File not found.");
        }

        try
        {
            var configuration = System.IO.File.ReadAllText(path);

            LoadConfiguration(configuration);
        }
        catch (Exception ex)
        {
            throw new InvaidFileException(path, ex.Message);
        }
        
        return this;
    }

    /// <summary>
    ///     By calling this method, the builder object produces a mapping configuration object. 
    /// </summary>
    /// <returns>
    ///     Returns produced mapping configuration object depending on registered types, models,and 
    ///     assemblies.
    /// </returns>
    /// <exception cref="MappingConfigurationBuildingException"></exception>
    public MappingPlan Build()
    {
        if (_registeredAssemblies.Count == 0)
        {
            throw new MappingConfigurationBuildingException("The builder requires a list of registered assemblies.");
        }

        if (_jsonConfiguration == null)
        {
            return new MappingConfiguration(_registeredAssemblies);
        }

        return new MappingPlan(_registeredAssemblies, _jsonConfiguration);
    }
}
