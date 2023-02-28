public static class VaultFactory
{
    public static State CreateVault(string name, string masterPassword)
    {
        string projectPath = Environment.CurrentDirectory;
        string folderPath = Path.Combine(Path.Combine(projectPath, "Vaults"), name);


        // Create the vault folder
        // If the folder already exists, throw an exception
        if(!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        else 
            throw new Exception("Vault already exists");

        

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
        
        // Return the state of the vault
        State.SetState(new State(name, masterPassword, secretKey, IV));

        return State.CurrentState;
        
    }

    public static State LoadVault(string vaultName, string masterPassword)
    {
        // Get the path to the vault
        string projectPath = Environment.CurrentDirectory;
        string folderPath = Path.Combine(Path.Combine(projectPath, "Vaults"), vaultName);


        // Check if the vault exists
        if(!Directory.Exists(folderPath))
            throw new Exception("Vault does not exist");
    

        // Get the key from the client vault
        string secretKey = TextFileProcessor.GetKey(vaultName);
        if(secretKey == null)
            throw new Exception("Failed to get key");
        
        
        // Generate the full key from the master password and the secret key
        byte[] fullkey = Encryptor.GenerateFullKey(Encoding.UTF8.GetBytes(masterPassword), Encoding.UTF8.GetBytes(secretKey));

        var VaultLoad = TextFileProcessor.Load(vaultName, fullkey);
        
        if(VaultLoad.Success == false)
            throw new Exception("Failed to load vault");
        
        // Return the state of the vault
        State.SetState(new State(vaultName, masterPassword, secretKey, VaultLoad.IV));
        State.CurrentState.SetLogins(VaultLoad.logins);
        State.CurrentState.Success = true;

        return State.CurrentState;

    }
}