namespace InForm.Server.Core.Features.Common;

public interface IVisitable
{
    void Accept(IVisitor visitor);
    TResult? Accept<TResult>(IVisitor<TResult> visitor) 
        where TResult : notnull;
}
