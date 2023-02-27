
public static class Decryptor
{
    public static string Decrypt(byte[] encryptedText, byte[] key, byte[] iv)
    {
        byte[] decryptedData;

        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using(var decryptor = aes.CreateDecryptor())
            {
                decryptedData = decryptor.TransformFinalBlock(encryptedText, 0, encryptedText.Length);
            }

            return Encoding.UTF8.GetString(decryptedData);
        }
    }
    
}