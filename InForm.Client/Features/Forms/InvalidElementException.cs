namespace InForm.Client.Features.Forms;

/// <summary>
///     Exception representing that the given element was not in an acceptable 
///     condition for the operation.
/// </summary>
/// <param name="element">The element in an invalid state.</param>
/// <param name="message">Optional textual message about the error.</param>
public class InvalidElementException(
    ElementModel element,
    string? message = null
) : ApplicationException(message)
{
    public ElementModel Element { get; } = element;
}
