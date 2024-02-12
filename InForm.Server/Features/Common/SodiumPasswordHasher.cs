using System.Text;
using Geralt;
using Microsoft.Extensions.Options;

namespace InForm.Server.Features.Common;

internal class SodiumPasswordHasher(IOptionsMonitor<SodiumHasherOptions> options) : IPasswordHasher
{
    private int Iterations => options.CurrentValue.Iterations;

    private int Memory => options.CurrentValue.Memory;

    public string Hash(string passwd) => Hash(passwd, Memory, Iterations);

    public string Hash(string passwd, int memory, int iterations)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(passwd);
        Span<byte> hash = stackalloc byte[Argon2id.MaxHashSize];
        Argon2id.ComputeHash(hash, passwordBytes, iterations, memory);
        var stringHash = Encoding.UTF8.GetString(hash);
        return string.Concat(stringHash.TakeWhile(x => x != '\0'));
    }

    public HashVerificationResult VerifyAndUpdate(string passwd, string hash)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(passwd);
        var hashBytes = Encoding.UTF8.GetBytes(hash);
        var succ = Argon2id.VerifyHash(hashBytes, passwordBytes);
        if (!succ) return HashVerificationResult.Failed;

        if (!Argon2id.NeedsRehash(hashBytes, Iterations, Memory))
            return HashVerificationResult.Succeeded;
        return HashVerificationResult.NeedsRehash(Hash(passwd));
    }
}

internal class SodiumHasherOptions
{
    public int Iterations { get; set; } = 4;
    public int Memory { get; set; } = 67108864;
}