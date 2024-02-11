namespace InForm.Server.Features.FillForms;

internal class InvalidElementTypeException(string expected, string got) : InvalidOperationException
{
    public string Expected { get; } = expected;
    public string Got { get; } = got;
}
