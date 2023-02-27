public class State
{
    private static State _CurrentState { get; set; }
    public Dictionary<string, Login> Passwords { get; set; }

    public string Name { get; set; }

    // says if the vault was unlocked successfully
    public bool Success { get; set; }

    // Inital Vector for the AES encryption
    public byte[] IV { get; set; }

    // The key for the AES encryption
    // Derived from password and secret key 
    public byte[] fullkey { get; set; }


    public State(string name, string masterPassword, string secretKey, byte[] IV)
    {
        Name = name;
        this.IV = IV;
        fullkey = Encryptor.GenerateFullKey(Encoding.UTF8.GetBytes(masterPassword), Encoding.UTF8.GetBytes(secretKey));
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

    public static void SetState(State state)
    {
        if(_CurrentState != null)
            _CurrentState.Save();
        _CurrentState = state;
    }

    public bool Save()
    {
        List<Login> logins = new List<Login>();
        foreach (var login in Passwords)
        {
            logins.Add(login.Value);
        }
        return TextFileProcessor.Save(logins, fullkey, IV, Name);
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