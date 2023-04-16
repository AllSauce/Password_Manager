

public static class TextFileProcessor 
{
    public static bool Save(List<Login> objectsToSave, byte[] fullkey, byte[] IV, string ServerVaultFileName)
    {
           
        
        try
        {
            List<string> lines = new List<string>();    

            foreach (var login in objectsToSave)
            {
                string line = $"{login.Username},{login.Password},{login.Website},{login.Notes}";
                var EncryptedLine = Encryptor.Encrypt(line, fullkey, IV);
                lines.Add(Convert.ToBase64String(EncryptedLine));
            }

            // Add the IV to the top of the file
            lines.Insert(0, Convert.ToBase64String(IV));

            // If the file is successfully decrypted then this line will be true
            lines.Insert(1, Convert.ToBase64String(Encryptor.Encrypt("true", fullkey, IV)));

            // Write the lines to the file
            File.WriteAllLines(ServerVaultFileName, lines);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }

        return true;
    }

    public static bool SaveKey(string key, string path)
    {
        
        try
        {
            File.WriteAllText(path , key);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }

        return true;
    }

    public static string GetKey(string ClientVaultFilePath)
    {
        
        if(!File.Exists(ClientVaultFilePath))
            throw new FileNotFoundException("Could not find client file");
        else if(File.ReadAllText(ClientVaultFilePath) == "")
            throw new Exception("The client file is empty");
        else if(File.ReadLines(ClientVaultFilePath).Count() > 1 || File.ReadLines(ClientVaultFilePath).Count() < 1)
            throw new Exception("The client file is not formatted correctly");
        else
            return File.ReadAllText(ClientVaultFilePath);
        
        
        
        
    }
    

    public static vaultLoad Load(string filename, byte[] fullkey)
    {
        vaultLoad output = new vaultLoad();
        output.Success = false;
        output.logins = new List<Login>();
        output.IV = new byte[16];

        try
        {
            //Read the file
            string[] lines = File.ReadAllLines(filename);

            //Get the IV from the top of the file
            output.IV = Convert.FromBase64String(lines[0]);

            //If the file is successfully decrypted then this line will be true
            string success = Decryptor.Decrypt(Convert.FromBase64String(lines[1]), fullkey, output.IV);

            if (success == "true")
            {
                //Since the file is successfully decrypted set the success to true
                output.Success = true;

                //Remove the IV and success line from the file
                lines = lines.Skip(2).ToArray();

                foreach (var line in lines)
                {
                    //Decrypt the line
                    string decryptedLine = Decryptor.Decrypt(Convert.FromBase64String(line), fullkey, output.IV);

                    //Split the line into its parts
                    string[] parts = decryptedLine.Split(',');

                    //Create a new login
                    Login login = new Login(parts[0], parts[1], parts[2], parts[3]);

                    //Add the login to the list
                    output.logins.Add(login);
                }
            }
            else throw new Exception("The file is not successfully decrypted, are you using the right password?");
        }
        catch(Exception e)
        {
            //If there is an error set the success to false
            Console.WriteLine(e.Message);
            output.Success = false;
        }

        return output;
    }
    
}   
        

       
        


