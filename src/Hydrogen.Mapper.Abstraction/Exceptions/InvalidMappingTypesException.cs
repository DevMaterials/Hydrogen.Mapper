using Hydrogen.Abstraction.Exceptions;

namespace Hydrogen.Mapper.Abstraction.Exceptions;

public class InvalidMappingTypesException(string sourceTypeName, string destinationTypeName) 
    : AbstractException("At least, one of the specified types are not available or appropriate for mapping.")
{
    public string SourceTypeName { get; } = sourceTypeName;
    public string DestinationTypeName { get; } = destinationTypeName;
}
