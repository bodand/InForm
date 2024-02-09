namespace InForm.Features.Forms.Db;

public class StringFormElement : FormElementBase
{
    public bool RenderAsTextArea { get; set; }
    public long? MaxLength { get; set; }
}