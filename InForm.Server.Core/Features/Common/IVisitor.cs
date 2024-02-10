namespace InForm.Server.Core.Features.Common;

public interface IVisitor<out TResult>
{
}

public interface IVisitor
{
}

public interface ITypedVisitor<in TVisited, out TResult> : IVisitor<TResult>
    where TVisited : IVisitable
{
    TResult Visit(TVisited visited);
}

public interface ITypedVisitor<in TVisited> : IVisitor
    where TVisited : IVisitable
{
    void Visit(TVisited visited);
}
