public static class VaultFactory
{
    public static State CreateVault(string clientPath, string serverPath, string masterPassword)
    {


        // Generate the key for the vault
        string secretKey = KeyGenerator.Instance.GenerateKey();
        
        // Store the key in the client vault
        if(!TextFileProcessor.SaveKey(secretKey, clientPath))
            throw new Exception("Failed to save key");
        
        // Generate the full key from the master password and the secret key
        byte[] fullkey = Encryptor.GenerateFullKey(Encoding.UTF8.GetBytes(masterPassword), Encoding.UTF8.GetBytes(secretKey));


        // Generate the IV for the vault
        byte[] IV = KeyGenerator.Instance.GenerateIV();

        // Save the vault
        if(!TextFileProcessor.Save(new List<Login>(), fullkey, IV, serverPath))
            throw new Exception("Failed to save vault");
        
        // Return the state of the vault
        State.SetState(new State( serverPath, masterPassword, secretKey, IV));

        return State.CurrentState;
        
    }

    public static State LoadVault(string serverPath, string clientPath, string masterPassword)
    {
    
        



    
        // Check if the vault exists   
        if(!File.Exists(serverPath))
            throw new Exception("No file exists at serverpath");

        if(!File.Exists(clientPath))
            throw new Exception("No file exists at clientpath");


        // Get the key from the client vault
        string secretKey = TextFileProcessor.GetKey(clientPath);
        if(secretKey == null)
            throw new Exception("Failed to get key");
        

        // Generate the full key from the master password and the secret key
        byte[] fullkey = Encryptor.GenerateFullKey(Encoding.UTF8.GetBytes(masterPassword), Encoding.UTF8.GetBytes(secretKey));

        var VaultLoad = TextFileProcessor.Load(serverPath, fullkey);
        
        if(VaultLoad.Success == false)
            throw new WrongPasswordException(masterPassword);
        
        // Return the state of the vault
        State.SetState(new State(clientPath, masterPassword, secretKey, VaultLoad.IV));
        State.CurrentState.SetLogins(VaultLoad.logins);
        State.CurrentState.Success = true;

        return State.CurrentState;

    }

    //Alternate LoadVault where secretKey is input mechanically
    public static State LoadVaultWithSecretKey(string serverPath, string masterPassword, string secretKey)
    {
    
        
        // Check if the vault exists   
        if(!File.Exists(serverPath))
            throw new Exception("No file exists at serverpath");
        

        // Generate the full key from the master password and the secret key
        byte[] fullkey = Encryptor.GenerateFullKey(Encoding.UTF8.GetBytes(masterPassword), Encoding.UTF8.GetBytes(secretKey));

        var VaultLoad = TextFileProcessor.Load(serverPath, fullkey);
        
        if(VaultLoad.Success == false)
            throw new Exception("Failed to load vault");
        
        // Return the state of the vault
        State.SetState(new State("Openvault", masterPassword, secretKey, VaultLoad.IV));
        State.CurrentState.SetLogins(VaultLoad.logins);
        State.CurrentState.Success = true;

        return State.CurrentState;

    }
}