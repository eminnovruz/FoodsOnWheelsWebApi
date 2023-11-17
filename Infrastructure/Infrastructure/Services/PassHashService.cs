using Application.Services;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services;

public class PassHashService : IPassHashService
{
    public void Create(string password, out byte[] PassHash, out byte[] PassSalt)
    {
        using var hmac = new HMACSHA512();
        PassSalt = hmac.Key;
        PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public bool ConfirmPasswordHash(string password, byte[] PassHash, byte[] PassSalt)
    {
        using var hmac = new HMACSHA512(PassSalt);
        var compHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return compHash.SequenceEqual(PassHash);
    }
}
