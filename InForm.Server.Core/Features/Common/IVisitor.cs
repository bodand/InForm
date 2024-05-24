namespace InForm.Server.Core.Features.Common;

/// <summary>
///     A notational interface for an acyclic-visitor. The action performed by
///     the visitor returns <c>TResult</c>.
/// </summary>
/// <typeparam name="TResult">
///     The type of the value returned by the visitor. The visitor may return
///     null. Covariant.
/// </typeparam>
public interface IVisitor<out TResult>;

/// <summary>
///     A notational interface for an acyclic-visitor. The action performed by
///     the visitor returns nothing.
/// </summary>
public interface IVisitor;

/// <summary>
///     A typed visitor interface for an acyclic-visitor.
/// </summary>
/// <remarks>
///     The type parameters are the visited type and the returned type for that
///     visitation. If a type wishes to visit multiple types, it must implement
///     this interface multiple times, with the respective input and output
///     types in the type-parameter list.
/// </remarks>
/// <typeparam name="TVisited">The type to visit. Contravariant.</typeparam>
/// <typeparam name="TResult">The return type of the visitation. Covariant.</typeparam>
public interface ITypedVisitor<in TVisited, out TResult> : IVisitor<TResult>
    where TVisited : IVisitable
{
    /// <summary>
    ///     Performs the visitation on the given object.
    /// </summary>
    /// <param name="visited">The visited object.</param>
    /// <returns>The result of the visitation.</returns>
    TResult Visit(TVisited visited);
}

/// <summary>
///     A typed visitor interface for an acyclic-visitor. Does not generate a
///     value in the visitation.
/// </summary>
/// <remarks>
///     The type parameters are the visited type and the returned type for that
///     visitation. If a type wishes to visit multiple types, it must implement
///     this interface multiple times, with the respective input types as the
///     type-parameter.
/// </remarks>
/// <typeparam name="TVisited">The type to visit. Contravariant.</typeparam>
public interface ITypedVisitor<in TVisited> : IVisitor
    where TVisited : IVisitable
{
    /// <summary>
    ///     Performs the visitation on the given object.
    /// </summary>
    /// <param name="visited">The visited object.</param>
    void Visit(TVisited visited);
}
