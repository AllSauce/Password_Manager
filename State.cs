public class State
{
    private static State _CurrentState { get; set; }
    public Dictionary<string, Login> Passwords { get; set; }

    // Name of the vault currently open
    public string ServerPath { get; set; }

    // says if the vault was unlocked successfully
    public bool Success { get; set; }

    // Inital Vector for the AES encryption
    public byte[] IV { get; set; }

    // The key for the AES encryption
    // Derived from password and secret key 
    public byte[] fullkey { get; set; }


    public State(string ServerPath, string masterPassword, string secretKey, byte[] IV)
    {
        this.ServerPath = ServerPath;
        this.IV = IV;
        try{
            fullkey = Encryptor.GenerateFullKey(Encoding.UTF8.GetBytes(masterPassword), Encoding.UTF8.GetBytes(secretKey));
        }
        catch(ArgumentNullException e)
        {
            switch(e.Message)
            {
                case "MasterPassword":
                    Console.WriteLine("MasterPassword cannot be null");
                    Environment.Exit(1);
                    break;
                case "key":
                    Console.WriteLine("Key cannot be null");
                    Environment.Exit(1);
                    break;
                default:
                    Console.WriteLine("Failed to generate key");
                    Environment.Exit(1);
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to generate key");
            Console.WriteLine(e.Message);
            Environment.Exit(1);
        }
        Passwords = new Dictionary<string, Login>();
        _CurrentState = this;
    }

    public void AddLogin(Login login)
    {
        Passwords.Add(login.Property, login);
    }

    public void RemoveLogin(string property)
    {
        if(Passwords.ContainsKey(property))
            Passwords.Remove(property);
        else 
            throw new Exception("Login does not exist");

        if(_CurrentState is not null)
            _CurrentState.Save();
    }

    public void SetLogins(List<Login> logins)
    {
        Passwords = new Dictionary<string, Login>();
        foreach (var login in logins)
        {
            Passwords.Add(login.Property, login);
        }
    }

    public void setLoginPassword(string property, string newPass)
    {
        if(Passwords.Count > 0 && Passwords.ContainsKey(property))
        {
            Login temp = new Login(Passwords[property].Property, Passwords[property].Password);

            RemoveLogin(property);

            AddLogin(temp);
        }
        else 
            AddLogin(new Login(property, newPass));

        

        if(_CurrentState is not null)
            _CurrentState.Save();
    }

    public static void SetState(State state)
    {
        // Save the current state

        // Set the new state
        _CurrentState = state;
    }

    public bool Save()
    {
        List<Login> logins = new List<Login>();
        foreach (var login in Passwords)
        {
            logins.Add(login.Value);
        }
        return TextFileProcessor.Save(logins, fullkey, IV, ServerPath);
    }

    public Login GetLogin(string property)
    {
        if(Passwords.ContainsKey(property))
            return Passwords[property];
        else
            throw new Exception("Login does not exist");
        
    }

    public static State CurrentState
    {
        get
        {
            if (_CurrentState == null)
                throw new Exception("No vault is currently open");
            else
                return _CurrentState;
        }
      
    }

}