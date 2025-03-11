using System.Security.Cryptography;
using System.Text;

namespace OrderProcessing.Common.Helpers;

public static class EncryptionHelper
{
    private static readonly string EncryptionKey = "Your32ByteEncryptionKeyForAES256"; // 32-byte key for AES-256

    public static string Encrypt(string plaintext)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = GenerateKeyFromPassphrase(EncryptionKey); // Ensure AES-256 (32-byte key)
            aesAlg.IV = new byte[16]; // Zero IV for simplicity, you should use a random IV in production

            using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
            using (var msEncrypt = new MemoryStream())
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plaintext);  // Write the plaintext to the stream
                swEncrypt.Flush();           // Ensure everything is written to the stream
                csEncrypt.FlushFinalBlock(); // Ensure the final block is encrypted

                return Convert.ToBase64String(msEncrypt.ToArray()); // Convert to base64 string
            }
        }
    }

    public static string Decrypt(string ciphertext)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = GenerateKeyFromPassphrase(EncryptionKey);
            aesAlg.IV = new byte[16]; // Zero IV for simplicity

            using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
            using (var msDecrypt = new MemoryStream(Convert.FromBase64String(ciphertext)))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();  // Read the decrypted text from the stream
            }
        }
    }

    private static byte[] GenerateKeyFromPassphrase(string passphrase)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(passphrase)); // Hash the passphrase to generate a 32-byte key
        }
    }
}
