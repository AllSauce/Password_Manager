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
        Passwords.Add(login.Website, login);
    }

    public void RemoveLogin(string website)
    {
        Passwords.Remove(website);
    }

    public void SetLogins(List<Login> logins)
    {
        Passwords = new Dictionary<string, Login>();
        foreach (var login in logins)
        {
            Passwords.Add(login.Website, login);
        }
    }

    public void setLoginPassword(string website, string newPass)
    {
        if(Passwords.Count > 0 && Passwords.ContainsKey(website))
        {
            Login temp = new Login(Passwords[website].Username, Passwords[website].Password, website, Passwords[website].Notes);

            RemoveLogin(website);

            AddLogin(temp);
        }

        if(_CurrentState != null)
            _CurrentState.Save();
    }

    public static void SetState(State state)
    {
        // Save the current state
        if(_CurrentState != null)
            _CurrentState.Save();

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

    public Login GetLogin(string website)
    {
        if(Passwords.ContainsKey(website))
            return Passwords[website];
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