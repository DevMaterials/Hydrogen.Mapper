using Hydrogen.Abstraction.Exceptions;

namespace Hydrogen.Mapper.Exceptions;

public class MappingConfigurationBuildingException(string message) : AbstractException(message)
{
}
