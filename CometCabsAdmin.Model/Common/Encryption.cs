using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CometCabsAdmin.Model.Contracts;

namespace CometCabsAdmin.Model.Common
{
    public class Encryption : IEncryption
    {
        private byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        private byte[] iv16Bit = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        #region IEncryption Members

        public string Encrypt(string dataToEncrypt)
        {
            var bytes = Encoding.Default.GetBytes(dataToEncrypt);
            using (var aes = new AesCryptoServiceProvider())
            {
                using (var ms = new MemoryStream())
                {
                    using (var encryptor = aes.CreateEncryptor(key, iv16Bit))
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(bytes, 0, bytes.Length);
                            cs.FlushFinalBlock();
                            var cipher = ms.ToArray();

                            return Convert.ToBase64String(cipher);
                        }
                    }
                }
            }
        }

        public string Decrypt(string dataToDecrypt)
        {
            var bytes = Convert.FromBase64String(dataToDecrypt);
            using (var aes = new AesCryptoServiceProvider())
            {
                using (var ms = new MemoryStream())
                {
                    using (var decryptor = aes.CreateDecryptor(key, iv16Bit))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(bytes, 0, bytes.Length);
                            cs.FlushFinalBlock();
                            var cipher = ms.ToArray();

                            return Encoding.UTF8.GetString(cipher);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
