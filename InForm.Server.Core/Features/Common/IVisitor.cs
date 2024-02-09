namespace InForm.Server.Core.Features.Common;

public interface IVisitor<out TResult>
{
}

public interface IVisitor
{
}

public interface ITypedVisitor<in TVisited, out TResult> : IVisitor<TResult>
{
    TResult Visit(TVisited visited);
}

public interface ITypedVisitor<in TVisited> : IVisitor
{
    void Visit(TVisited visited);
}
