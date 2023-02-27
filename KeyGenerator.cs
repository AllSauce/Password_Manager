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

    public byte[] GenerateKeyBytes()
    {
        var key = new byte[32];
        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(key);
        }
        return key;
    }

    public byte[] GenerateIV()
    {
        var IV = new byte[16];
        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(IV);
        }
        return IV;
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

    private KeyGenerator()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        
    }
}