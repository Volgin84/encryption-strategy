using System.Security.Cryptography;
using System.Text;

namespace Strategy
{
    public class CaesarCipher : IEncryptionStrategy // First concrete strategy
    {
        public string Encrypt(string text)
        {
            StringBuilder cipherText = new();
            int shift = 3;
            foreach (char ch in text)
            {
                char shifted = (char)(ch + shift);
                cipherText.Append(shifted);
            }
            return cipherText.ToString();
        }
    }

    public class RSACipher : IEncryptionStrategy // Second concrete strategy
    {
        public string Encrypt(string text)
        {
            try
            {
                using var rsa = new RSACryptoServiceProvider();
                byte[] textBytes = Encoding.UTF8.GetBytes(text);
                byte[] encryptedText = rsa.Encrypt(textBytes, false);
                return Convert.ToBase64String(encryptedText);
            }
            catch (CryptographicException e)
            {
                throw new Exception($"RSA Encryption failed: {e.Message}");
            }
        }
    }

    public class AESCipher : IEncryptionStrategy // Third concret strategy
    {
        public string Encrypt(string text)
        {
            try
            {
                using Aes aes = Aes.Create();
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] textBytes = Encoding.UTF8.GetBytes(text);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);
                return Convert.ToBase64String(encryptedBytes);
            }
            catch (CryptographicException e)
            {
                throw new Exception($"AES Encryption failed: {e.Message}");
            }
        }
    }

    public class NoEncryption : IEncryptionStrategy // Fourht concrete strategy
    {
        public string Encrypt(string text)
        {
            return text;
        }
    }
}
