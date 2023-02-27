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
    public byte[] key { get; set; }


    public State(string name, string masterPassword, string secretKey, byte[] IV)
    {
        Name = name;
        this.IV = IV;
        key = Encryptor.GenerateFullKey(Encoding.UTF8.GetBytes(masterPassword), Encoding.UTF8.GetBytes(secretKey));
        Passwords = new Dictionary<string, Login>();
        _CurrentState = this;
    }

    public State CurrentState
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