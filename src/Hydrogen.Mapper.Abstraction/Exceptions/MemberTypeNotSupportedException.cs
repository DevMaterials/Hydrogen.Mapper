using Hydrogen.Abstraction.Exceptions;
using System.Reflection;

namespace Hydrogen.Mapper.Abstraction.Exceptions;

public class MemberTypeNotSupportedException(MemberTypes memberType) : AbstractException
{
    public MemberTypes MemberType { get; } = memberType;
}
