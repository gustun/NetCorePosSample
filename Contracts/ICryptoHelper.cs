using System;
using System.Collections.Generic;
using System.Text;

namespace Pos.Contracts
{
    public interface ICryptoHelper
    {
        string Hash(string input);
        bool Verify(string input, string hashedInput);
    }
}
