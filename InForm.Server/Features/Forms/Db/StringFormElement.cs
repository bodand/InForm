using InForm.Server.Core.Features.Common;

namespace InForm.Features.Forms.Db;

public class StringFormElement : FormElementBase
{
    public bool RenderAsTextArea { get; set; }
    public int? MaxLength { get; set; }

    public override void Accept(IVisitor visitor)
    {
        if (visitor is not ITypedVisitor<StringFormElement> typedVisitor) return;
        typedVisitor.Visit(this);
    }

    public override TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : default
    {
        if (visitor is not ITypedVisitor<StringFormElement, TResult> typedVisitor) return default;
        return typedVisitor.Visit(this);
    }
}