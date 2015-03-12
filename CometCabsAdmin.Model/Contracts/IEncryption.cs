using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CometCabsAdmin.Model.Contracts
{
    public interface IEncryption
    {
        string Encrypt(string dataToEncrypt);
        string Decrypt(string dataToDecrypt);
    }
}
