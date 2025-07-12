using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.Utils;

public static class Descryptography
{
    private const string SecretKey = "3CEF74DA196C86D0E55A26FEE2D65BA6E8F712F1F88AAF48A884E7CD0C7BE670";

    public static string Descrypto(string encryptedPassword)
    {
        // Convertir el texto encriptado de Base64 a bytes
        byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);

        // Calcular el hash a partir del secreto
        string hash = ComputeHash(SecretKey.Substring(0, 32));
        byte[] key = Encoding.UTF8.GetBytes(hash); // 32 bytes
        byte[] iv = new byte[] { 33, 24, 31, 46, 75, 64, 97, 18, 89, 10, 111, 132, 131, 144, 145, 250 }; // 16 bytes

        // Set up the decryption objects
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Decrypt the input ciphertext using the AES algorithm
            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            {
                // Decryption buffer
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                // Convertir bytes a texto
                string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

                return decryptedText;
            }
        }
    }

    private static string ComputeHash(string input)
    {
        using (var md5 = MD5.Create())
        {
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in data)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
