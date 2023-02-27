public static class Encryptor
{
    public static byte[] Encrypt(string plainText, byte[] key, byte[] iv)
    {
        byte[] encryptedData;
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using (var encryptor = aes.CreateEncryptor())
            {
                encryptedData = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(plainText), 0, plainText.Length);
            }
        }

        return encryptedData;
    }


    // This method will generate the full key from the master password and the key
    //Using rfc2898DeriveBytes
    public static byte[] GenerateFullKey(byte[] MasterPassword, byte[] key)
    {
        byte[] fullKey;
        using (var deriveBytes = new Rfc2898DeriveBytes(MasterPassword, key, 10000, HashAlgorithmName.SHA256))
        {
            fullKey = deriveBytes.GetBytes(32);
        }

        return fullKey;
    }
}