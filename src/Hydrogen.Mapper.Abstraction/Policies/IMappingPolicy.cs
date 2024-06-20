namespace Hydrogen.Mapper.Abstraction.Policies;

public interface IMappingPolicy;
public interface IMappingPolicy<TConfig> : IMappingPolicy
{
    TConfig Value { get; }
}
