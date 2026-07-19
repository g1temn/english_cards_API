using System.Security.Cryptography;

namespace englishCardsAPI.Utils
{
    static public class RefreashTokenGenerator
    {
        static public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
