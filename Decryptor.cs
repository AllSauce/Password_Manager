
public static class Decryptor
{
    public static string Decrypt(byte[] encryptedText, byte[] key, byte[] iv)
    {
        byte[] decryptedData;
        if(encryptedText == null || encryptedText.Length <= 0)
            throw new ArgumentNullException("encryptedText");
        if(key == null || key.Length <= 0)
            throw new ArgumentNullException("key");
        if(iv == null || iv.Length <= 0)
            throw new ArgumentNullException("iv");

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