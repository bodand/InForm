namespace InForm.Server.Features.Common.Security;

/// <summary>
///     An interface for a class, that has its access controlled by a password.
///     Allows the use of the higher level IPasswordHandler interface.
/// </summary>
public interface IPasswordControlled {
    /// <summary>
    ///     The hash of the password controlling the access to this entity.
    ///     A null hash means that the entity is not actually handled by a
    ///     password.
    /// </summary>
    string? PasswordHash { get; set; }
}
