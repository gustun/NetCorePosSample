using System.Text;
using Pos.Core.Interface;

namespace Pos.Utility
{
    public class CryptoHelper : ICryptoHelper
    {
        public string Hash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        public bool Verify(string input, string hashedInput)
        {
            return Hash(input) == hashedInput;
        }
    }
}
