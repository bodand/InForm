namespace InForm.Features.Forms.Db;

public class RangeFormElement : FormElementBase
{
    public int Minimum { get; set; }
    public int Maximum { get; set; }
    public int Step { get; set; } = 1;
}