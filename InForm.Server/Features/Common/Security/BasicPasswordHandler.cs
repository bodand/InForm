using Microsoft.EntityFrameworkCore;

namespace InForm.Server.Features.Common.Security;

/// <summary>
///     The default implementation of the password handler interface.
/// </summary>
public class BasicPasswordHandler(IPasswordHasher hasher) : IPasswordHandler {

    /// <inheritdoc />
    public async Task<T?> Verify<T>(T? obj, string? password, DbContext dbContext)
        where T : class, IPasswordControlled
    {
        var result = PerformVerify(obj, password);
        if (result is null) return result;

        await dbContext.SaveChangesAsync();
        return result;
    }

    private T? PerformVerify<T>(T? obj, string? password)
        where T : class, IPasswordControlled
    {
        if (obj?.PasswordHash is not {} hash) return null;

        var passResult = hasher.VerifyAndUpdate(password ?? string.Empty, hash);
        if (!passResult.Verified) return null;

        if (passResult is not { UpdatedHash: {} newHash }) return obj;

        obj.PasswordHash = newHash;
        return obj;
    }

    /// <inheritdoc />
    public T? VerifyWithoutSaving<T>(T? obj, string? password)
        where T : class, IPasswordControlled
        => PerformVerify(obj, password);
}
