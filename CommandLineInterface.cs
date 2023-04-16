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
        try{
            switch(args[0])
            {
                case "init":
                    if(args.Length != 3) throw new WrongInputException("init");
                    Init(args[1], args[2]);
                    break;
                case "create":
                    if(args.Length != 3) throw new WrongInputException("create");
                    Create(args[1], args[2]);
                    break;
                case "get":
                    if(args.Length != 4) throw new WrongInputException("get");
                    Get(args[1], args[2], args[3]);
                    break;
                case "set":
                    if(args.Length != 5) throw new WrongInputException("set");
                    Set(args[1], args[2], args[3], args[4]);
                    break;
                case "delete":
                    if(args.Length != 4) throw new WrongInputException("delete");
                    Delete(args[1], args[2], args[3]);
                    break;
                case "secret":
                    if(args.Length != 2) throw new WrongInputException("secret");
                    Secret(args[1]);
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }catch(WrongInputException)
        {
            Environment.Exit(1);
        }
    }

    //Unnecessary with string[] args
    private static string[] Cut(string s)
    {
        string[] arr = s.Split(' ');

        return arr;
    }

    private static void Init(string clientPath, string serverPath)
    {
        Console.WriteLine("Please enter your master-password: ");
        string masterPassword = Console.ReadLine() ?? "";
        try{
            VaultFactory.CreateVault(clientPath, serverPath, masterPassword);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Environment.Exit(1);
        }
        
        // Console.WriteLine(TextFileProcessor.GetKey(clientPath));
        //State.CurrentState.Save();
    }

    private static void Create(string clientPath, string serverPath)
    {
        Console.WriteLine("Please enter your master-password: ");
        string masterPassword = Console.ReadLine() ?? "";
        Console.WriteLine("Please enter the secret-key");
        string secretKey = Console.ReadLine() ?? ""; 
        
        VaultFactory.LoadVaultWithSecretKey(serverPath, masterPassword, secretKey);
        if(State.CurrentState.Success)
        {
            TextFileProcessor.SaveKey(secretKey, clientPath);
            State.CurrentState.ServerPath = serverPath;
        }
        else
        {
            Console.WriteLine("Failed to load vault");
        }
    }

    private static void Get(string clientPath, string serverPath, string property)
    {
        Console.WriteLine("Please enter your master-password: ");
        string masterPassword = Console.ReadLine() ?? "";

        VaultFactory.LoadVault(serverPath, clientPath, masterPassword);

        Console.WriteLine(State.CurrentState.GetLogin(property).Password);
    }

    private static void Set(string clientPath, string serverPath, string property, string gen)
    {
        if(gen == "-g" || gen == "--generate")
        {
            Console.WriteLine(PasswordGenerator.GeneratePassword());
            return;
        }
        Console.WriteLine("Please enter your master-password: ");
        string masterPassword = Console.ReadLine() ?? "";
        Console.WriteLine("Please enter the password you wish to store: ");
        string newPassword = Console.ReadLine() ?? "";

        VaultFactory.LoadVault(serverPath, clientPath, masterPassword);

        State.CurrentState.setLoginPassword(property, newPassword);
    }

    private static void Delete(string clientPath, string serverPath, string property)
    {
        Console.WriteLine("Please enter your master-password: ");
        string masterPassword = Console.ReadLine() ?? "";

        VaultFactory.LoadVault(serverPath, clientPath, masterPassword);

        State.CurrentState.RemoveLogin(property);
    }

    private static void Secret(string clientPath)
    {
        Console.WriteLine(TextFileProcessor.GetKey(clientPath));
    }
}