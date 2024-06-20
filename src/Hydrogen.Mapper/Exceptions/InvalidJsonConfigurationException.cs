using Hydrogen.Abstraction.Exceptions;

namespace Hydrogen.Mapper.Exceptions;

public class InvalidJsonConfigurationException(string configuration) : AbstractException
{
    public string Configuration { get; } = configuration;
}
