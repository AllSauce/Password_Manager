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

    public static void Command(string[] args)
    {
        /*
        string s = Console.ReadLine() ?? "";
        if(s == "") throw new Exception("Please enter a valid command with it's necessary components.");
        string[] arr = Cut(s);*/

        Console.ReadLine();

        switch(args[0])
        {
            case "init":
                Init(args[1], args[2], args[3]);
                break;
            case "create":
                Create(args[1], args[2]);
                break;
            case "get":
                Get(args[1], args[2], args[3]);
                break;
            case "set":
                Set();
                break;
            case "delete":
                Delete();
                break;
        }
    }

    //Unnecessary with string[] args
    private static string[] Cut(string s)
    {
        string[] arr = s.Split(' ');

        return arr;
    }

    private static void Init(string clientPath, string serverPath, string masterPassword)
    {
        VaultFactory.CreateVault(clientPath, serverPath, masterPassword);
    }

    private static void Create(string clientPath, string serverPath)
    {
        Console.WriteLine("Please enter your master-password: ");
        string masterPassword = Console.ReadLine() ?? "";
        Console.WriteLine("Please enter the secret-key");
        string secretKey = Console.ReadLine() ?? "";

        if(masterPassword == "" || secretKey == "") throw new NoInputException(masterPassword, secretKey); 
        
        State s = VaultFactory.LoadVaultWithSecretKey(serverPath, masterPassword, secretKey);
        if(s.Success)
        {
            TextFileProcessor.SaveKey(secretKey, clientPath);
        }
        else
        {
            throw new WrongInputException(masterPassword, secretKey);
        }
    }

    private static void Get(string clientPath, string serverPath, string property)
    {
        Console.WriteLine("Please enter your master-password: ");
        string masterPassword = Console.ReadLine() ?? "";

        State s = VaultFactory.LoadVault(serverPath, clientPath, masterPassword);

        //How do I get a password from under 'property'? 
        //Examples of property from canvas instructions: "username.example.com" or "password.example.com"
    }

    private static void Set()
    {

    }

    private static void Delete()
    {

    }

    private static void Secret()
    {
        
    }

    //Should provide a command guide:
    //Which ones there are and how to use them.
    private static void Help()
    {
        System.Console.WriteLine("The following commands are possible: ");
        System.Console.WriteLine();
        System.Console.WriteLine("init <client path> <server path> <master password>");
        System.Console.WriteLine(
            "The 'init' command creates a new 'client', 'server' and encrypts 'vault' stored in 'server' using 'master password'");
        System.Console.WriteLine();
        System.Console.WriteLine("create <client path> <server path> <master password> <secret key>");
        System.Console.WriteLine(
            "The 'create' command creates a new client that will be used to log in to server.");
            System.Console.WriteLine();
            
    }
}