namespace Pos.Core.Interface
{
    public interface ICryptoHelper
    {
        string Hash(string input);
        bool Verify(string input, string hashedInput);
    }
}
