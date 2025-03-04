using System.Security.Cryptography;

namespace BookAPI.Token
{
    public class Key
    {
        public static string Secret = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}
