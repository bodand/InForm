namespace InForm.Server.Features.Common;

/// <summary>
///     Exception class thrown when a service is instructed to perform work on
///     an entity that does not exist.
/// </summary>
internal class EntityNotFoundException : InvalidOperationException;
