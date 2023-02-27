public static class VaultFactory
{
    public static State CreateVault(string name, string masterPassword)
    {
        string projectPath = Environment.CurrentDirectory;
        string folderPath = Path.Combine(Path.Combine(projectPath, "Vaults"), name);

        if(!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        else 
            throw new Exception("Vault already exists");

        string ServerVaultFileName = Path.Combine(folderPath, "ServerVault.txt");
        string ClientVaultFileName = Path.Combine(folderPath, "ClientVault.txt");

        // Generate the key for the vault
        string secretKey = KeyGenerator.Instance.GenerateKey();
        
        // Store the key in the client vault
        if(!TextFileProcessor.SaveKey(secretKey, name))
            throw new Exception("Failed to save key");
        
        // Generate the full key from the master password and the secret key
        byte[] fullkey = Encryptor.GenerateFullKey(Encoding.UTF8.GetBytes(masterPassword), Encoding.UTF8.GetBytes(secretKey));


        // Generate the IV for the vault
        byte[] IV = KeyGenerator.Instance.GenerateIV();

        // Save the vault
        if(!TextFileProcessor.Save(new List<Login>(), fullkey, IV, name))
            throw new Exception("Failed to save vault");

        return new State(name, masterPassword, secretKey, IV);



        
    }
}