namespace InForm.Server.Core.Features.Common;

/// <summary>
///     The interface for classes that allow visitation by an
///     <see cref="IVisitor"/> or <see cref="IVisitor{TResult}"/>.
/// </summary>
public interface IVisitable
{
    /// <summary>
    ///     Accepts an untyped visitor that does not return a result.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <seealso cref="IVisitor"/>
    void Accept(IVisitor visitor);

    /// <summary>
    ///     Accepts a untyped visitor that generates a nullable value of the
    ///     type in its type parameter.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <typeparam name="TResult">
    ///     The result type of the visitor accepted.
    /// </typeparam>
    /// <returns>
    ///     The return value of the visitor, possibly a null value.
    /// </returns>
    /// <seealso cref="IVisitor{TResult}"/>
    TResult? Accept<TResult>(IVisitor<TResult> visitor) 
        where TResult : notnull;
}
