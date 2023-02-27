public class CommandLineInterface
{
    private static CommandLineInterface _instance; 


    public static CommandLineInterface Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CommandLineInterface();
            }
            return _instance;
        }
    }

    public static void Init()
    {

    }

    public static void Create()
    {

    }

    public static void Get()
    {

    }

    public static void Set()
    {

    }

    public static void Delete()
    {

    }

    public static void Secret()
    {
        
    }
}