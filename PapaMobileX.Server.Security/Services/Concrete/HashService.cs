using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PapaMobileX.Server.Security.Services.Interfaces;

namespace PapaMobileX.Server.Security.Services.Concrete;

public class HashService : IHashService
{
    public string GetHash(string password, string salt)
    {
        byte[] hashed = KeyDerivation.Pbkdf2(password,
                                             Convert.FromBase64String(salt),
                                             KeyDerivationPrf.HMACSHA256,
                                             100000,
                                             256 / 8);
        return Convert.ToBase64String(hashed);
    }

    public string GenerateSalt()
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        return Convert.ToBase64String(salt);
    }
}