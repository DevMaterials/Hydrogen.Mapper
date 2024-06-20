using Hydrogen.Mapper.Abstraction.Configuration;
using System.Linq.Expressions;

namespace Hydrogen.Mapper.Abstraction.Services;

/// <summary>This contract defines attributes and functionalities of expression mappers.</summary>
public interface IExpressionMapper
{
    /// <summary>This method makes a lambda expression by another expression.</summary>
    /// <typeparam name="TSource">The type of the source object</typeparam>
    /// <typeparam name="TDestination">The type of the target object</typeparam>
    /// <param name="expression">The source lambda expression depending on the source type.</param>
    /// <returns>Returns a lambda expression depending on the destination type.</returns>
    LambdaExpression Map<TSource, TDestination>(LambdaExpression expression);

    /// <summary>This method makes a lambda expression by another expression.</summary>
    /// <typeparam name="TSource">The type of the source object</typeparam>
    /// <typeparam name="TDestination">The type of the target object</typeparam>
    /// <param name="expression">The source lambda expression depending on the source type.</param>
    /// <param name="customPlan">A plan that will be used instead of the default plan.</param>
    /// <returns>Returns a lambda expression depending on the destination type.</returns>
    LambdaExpression Map<TSource, TDestination>(LambdaExpression expression, MappingPlan customPlan);
}
