namespace InForm.Client.Features.Forms;

/// <summary>
///     Exception class representing that the given form entity 
///     cannot be used to submit a fill.
/// </summary>
/// <param name="message">Textual representation of the error.</param>
public class InvalidFormToFillException(string? message) : ApplicationException(message)
{
}
