using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace EmployeeManagement.Services.Helpers;

public class PasswordHelper
{
    private const int SaltSize = 128 / 8;
    private const int KeySize = 256 / 8;
    private const int IterationCount = 100000;

    public static string HashPassword(string password)
    {
        // if (string.IsNullOrWhiteSpace(password))
        //     throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        byte[] hash = KeyDerivation.Pbkdf2(
             password: password,
             salt: salt,
             prf: KeyDerivationPrf.HMACSHA256,
             iterationCount: IterationCount,
             numBytesRequested: KeySize
        );

        string saltBase64 = Convert.ToBase64String(salt);
        string hashBase64 = Convert.ToBase64String(hash);

        string hashPasswordWithSalt = string.Join(".", hashBase64, saltBase64);
        return hashPasswordWithSalt;
    }

    public static bool VerifyPassword(string enterPassword, string storedPassword)
    {
        if (string.IsNullOrWhiteSpace(enterPassword) || string.IsNullOrWhiteSpace(storedPassword))
        {
            return false;
        }

        string[] saltAndPassword = storedPassword.Split('.');
        if (saltAndPassword.Length != 2)
        {
            return false;
        }

        byte[] saltbytes = Convert.FromBase64String(saltAndPassword[1]);
        byte[] expectedHashPassword  = Convert.FromBase64String(saltAndPassword[0]);

        byte[] actualHashPassword = KeyDerivation.Pbkdf2(
            password: enterPassword,
            salt: saltbytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
        );

        return CryptographicOperations.FixedTimeEquals(actualHashPassword, expectedHashPassword);
    }
}
