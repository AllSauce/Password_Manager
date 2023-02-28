public class CommandLineInterface
{
    private static CommandLineInterface? _instance; 


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

    public static void Command()
    {
        string s = Console.ReadLine() ?? "";
        if(s == "") throw new Exception("Please enter a valid command with it's necessary components.");
        string[] arr = Cut(s);

        switch(arr[0])
        {
            case "init":
                Init(arr[1], arr[2], arr[3]);
                break;
            case "create":
                Create();
                break;
            case "get":
                Get();
                break;
            case "set":
                Set();
                break;
            case "delete":
                Delete();
                break;
        }
    }

    private static string[] Cut(string s)
    {
        string[] arr = s.Split(' ');

        return arr;
    }

    public static void Init(string path1, string path2, string password)
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