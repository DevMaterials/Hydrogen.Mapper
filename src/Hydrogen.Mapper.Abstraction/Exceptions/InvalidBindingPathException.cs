using Hydrogen.Abstraction.Exceptions;

namespace Hydrogen.Mapper.Abstraction.Exceptions;

public class InvalidBindingPathException(string path) : AbstractException
{
    public string Path { get; } = path;
}
