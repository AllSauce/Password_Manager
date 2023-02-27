using System.Security.Cryptography;
public class KeyGenerator
{
    private static KeyGenerator _instance;
    public string GenerateKey()
    {
        var key = new byte[32];
        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(key);
        }
        return Convert.ToBase64String(key);
    }

    public static KeyGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new KeyGenerator();
            }
            return _instance;
        }
    }

    public KeyGenerator()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
}