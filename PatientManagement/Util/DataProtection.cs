using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PatientManagement.Util
{
    public static class EncryptionServices
    {
        private static string _salt = ConfigurationSettings.AppSettings["Salt"];
        private static string _privateKey = ConfigurationSettings.AppSettings["PrivateKey"];

        public static Aes _GetEncryptionAlgorithm(string additionalKey)
        {
            var salt = Encoding.ASCII.GetBytes(_salt);


            Aes aesEncryption = Aes.Create();
            aesEncryption.Mode = CipherMode.CBC;
            aesEncryption.Padding = PaddingMode.Zeros;

            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_privateKey + additionalKey, salt, 5);
            aesEncryption.Key = pdb.GetBytes(32);
            aesEncryption.IV = pdb.GetBytes(16);

            return aesEncryption;
        }

        public static string Decrypt(string textToDecrypt, string additionalKey)
        {
            byte[] valueBytes = Convert.FromBase64String(textToDecrypt);
            byte[] decryptedBytes;
            int decryptedByteCount = 0;

            using (Aes aesEncryption = _GetEncryptionAlgorithm(additionalKey))
            using (ICryptoTransform decryptor = aesEncryption.CreateDecryptor())
            {
                using (MemoryStream from = new MemoryStream(valueBytes))
                {
                    using (CryptoStream reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
                    {
                        decryptedBytes = new byte[valueBytes.Length];
                        decryptedByteCount = reader.Read(decryptedBytes, 0, decryptedBytes.Length);
                    }
                }
            }

            return Encoding.Unicode.GetString(decryptedBytes, 0, decryptedByteCount).TrimEnd('\0');
        }

        public static string Encrypt(string textToEncrypt, string additionalKey)
        {
            byte[] valueBytes = Encoding.Unicode.GetBytes(textToEncrypt);
            byte[] encryptedBytes;

            using (Aes aesEncryption = _GetEncryptionAlgorithm(additionalKey))
            using (ICryptoTransform encryptor = aesEncryption.CreateEncryptor())
            {
                using (MemoryStream to = new MemoryStream())
                {
                    using (CryptoStream writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
                        writer.Write(valueBytes, 0, valueBytes.Length);

                    encryptedBytes = to.ToArray();
                }
            }

            return Convert.ToBase64String(encryptedBytes);
        }
    }
}