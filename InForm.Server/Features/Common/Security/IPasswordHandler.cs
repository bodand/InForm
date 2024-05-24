using Microsoft.EntityFrameworkCore;

namespace InForm.Server.Features.Common.Security;

/// <summary>
///     A high-level handler for entities whose access may be controlled by a
///     password.
/// </summary>
public interface IPasswordHandler {
    /// <summary>
    ///     Verifies and optionally updates the password hash of the given
    ///     entity.
    ///     Calls SaveChangesAsync on the database context, to ensure the object
    ///     has its new password hash saved.
    ///     To do not perform saving <see cref="VerifyWithoutSaving{T}"/>.
    /// </summary>
    /// <param name="obj">The object to verify the password for.</param>
    /// <param name="password">The password to verify.</param>
    /// <param name="dbContext">
    ///     The context to call save on, to ensure the updated password hash
    ///     is properly stored in the database.
    /// </param>
    /// <typeparam name="T">The actual type of the controlled object.</typeparam>
    /// <returns>
    ///     The optionally updated entity if the password matched the current
    ///     hash. Null if the password provided is invalid.
    /// </returns>
    /// <seealso cref="VerifyWithoutSaving{T}"/>
    Task<T?> Verify<T>(T? obj, string? password, DbContext dbContext) where T : class, IPasswordControlled;

    /// <summary>
    ///     Verifies and optionally updates the password hash of the given
    ///     entity.
    ///     The entity needs to be manually persisted to the datastore it is
    ///     stored in.
    ///     To perform automatic saving <see cref="Verify{T}"/>.
    /// </summary>
    /// <param name="obj">The object to verify the password for.</param>
    /// <param name="password">The password to verify.</param>
    /// <typeparam name="T">The actual type of the controlled object.</typeparam>
    /// <returns>
    ///     The optionally updated entity if the password matched the current
    ///     hash. Null if the password provided is invalid.
    /// </returns>
    /// <seealso cref="VerifyWithoutSaving{T}"/>
    T? VerifyWithoutSaving<T>(T? obj, string? password) where T : class, IPasswordControlled;
}
