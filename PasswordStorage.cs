public static class PasswordStorage
{
    public static Dictionary<string, Login> _passwords = new Dictionary<string, Login>();

    public static Dictionary<string, Login> AddLogin(Login login)
    {
        _passwords.Add(login.Website, login);
        return _passwords;
    }
    
    public static Login getLogin(string website)
    {
        if(!_passwords.ContainsKey(website))
            throw new Exception("Website not found");
        return _passwords[website];
    }

    public static string GetPassword(string website)
    {
        if(!_passwords.ContainsKey(website))
            throw new Exception("Website not found");
        return _passwords[website].Password;
    }

}