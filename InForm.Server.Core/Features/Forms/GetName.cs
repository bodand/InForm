namespace InForm.Server.Core.Features.Forms;

/// <summary>
///     Response type for querying a forms name.
/// </summary>
/// <param name="Id">The id of the form.</param>
/// <param name="Title">The title of the form.</param>
/// <param name="Subtitle">The subtitle of the form.</param>
public readonly record struct GetFormNameResponse(
    Guid Id,
    string Title,
    string? Subtitle
);
