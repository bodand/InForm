namespace InForm.Server.Features.Common;

/// <summary>
///     Interface for securely hashing passwords.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    ///     Hashes the input password with the objects current settings.
    /// </summary>
    /// <param name="passwd">The password to hash.</param>
    /// <returns>The hashed password, secure for storage.</returns>
    string Hash(string passwd);

    /// <summary>
    ///     Hashes the input password with the given parameters.
    /// </summary>
    /// <param name="passwd">The cleartext password to hash.</param>
    /// <param name="memory">The amount of memory to use.</param>
    /// <param name="iterations">The amount of iterations to use.</param>
    /// <returns>The hashes password</returns>
    string Hash(string passwd, int memory, int iterations);
    
    /// <summary>
    ///     Verifies if a password is the same as the one used to generate the hash.
    ///     If the hashing options have changed, a new hash is computed using the password.
    /// </summary>
    /// <param name="passwd">The password to verify.</param>
    /// <param name="hash">The hash to verify against.</param>
    /// <returns>
    ///     A two element record containing whether the passwords match,
    ///     and an optional string (null if missing) if the password needs to be rehashed to
    ///     match the hasher object's current settings.
    /// </returns>
    HashVerificationResult VerifyAndUpdate(string passwd, string hash);
}

/// <summary>
///     The result object for verifying passwords.
/// </summary>
/// <param name="Verified">Whether the password value matches.</param>
/// <param name="UpdatedHash">
///     Non-null if the old hash is not up-to-date with the current parameters, in which case it contains the new
///     hash computed from the password.
/// </param>
public readonly record struct HashVerificationResult(
    bool Verified,
    string? UpdatedHash
)
{
    internal static readonly HashVerificationResult Failed = new(false, null);
    internal static readonly HashVerificationResult Succeeded = new(true, null);

    internal static HashVerificationResult NeedsRehash(string newHash) => new(true, newHash);
}