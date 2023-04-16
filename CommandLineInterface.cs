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
                    if(args.Length == 4) Set(args[1], args[2], args[3]);
                    else if(args.Length == 5) Set(args[1], args[2], args[3], args[4]);
                    else throw new WrongInputException("set");
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
        }
        catch(WrongInputException)
        {
            Console.WriteLine("Wrong input for command: " + args[0]);
            switch(args[0])
            {
                case "init":
                    Console.WriteLine("init <client-path> <server-path>");
                    break;
                case "create":
                    Console.WriteLine("create <client-path> <server-path>");
                    break;
                case "get":
                    Console.WriteLine("get <client-path> <server-path> <property>");
                    break;
                case "set":
                    Console.WriteLine("set <client-path> <server-path> <property> [-g]");
                    break;
                case "delete":
                    Console.WriteLine("delete <client-path> <server-path> <property>");
                    break;
                case "secret":  
                    Console.WriteLine("secret <client-path>");
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
            Environment.Exit(1);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
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
        try
        {
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
        try
        {
            VaultFactory.LoadVault(serverPath, clientPath, masterPassword);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Environment.Exit(1);
        }

        try
        {
            Console.WriteLine(State.CurrentState.GetLogin(property).Password);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Environment.Exit(1);
        }
        
    }

    private static void Set(string clientPath, string serverPath, string property)
    {
        Console.WriteLine("Please enter your master-password: ");
        string masterPassword = Console.ReadLine() ?? "";
        Console.WriteLine("Please enter the password you wish to store: ");
        string newPassword = Console.ReadLine() ?? "";
        try
        {
            VaultFactory.LoadVault(serverPath, clientPath, masterPassword);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Environment.Exit(1);
        }
        try
        {
            State.CurrentState.setLoginPassword(property, newPassword);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Environment.Exit(1);
        }
    }

    private static void Set(string clientPath, string serverPath, string property, string gen)
    {
        Console.WriteLine("Please enter your master-password: ");
        string masterPassword = Console.ReadLine() ?? "";

        string newPassword = PasswordGenerator.GeneratePassword();

        if(gen == "-g" || gen == "--generate")
        {
            try
            {
                VaultFactory.LoadVault(serverPath, clientPath, masterPassword);
                State.CurrentState.setLoginPassword(property, newPassword);
                Console.WriteLine("New password is: " + newPassword);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            
            return;
        }
        else
        {
            Console.WriteLine("Invalid argument.\n Type '-g' or '--generate' if you want an automatically generated password.");
            Environment.Exit(1);
        }
    }

    private static void Delete(string clientPath, string serverPath, string property)
    {
        Console.WriteLine("Please enter your master-password: ");
        string masterPassword = Console.ReadLine() ?? "";
        try
        {
            VaultFactory.LoadVault(serverPath, clientPath, masterPassword);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Environment.Exit(1);
        }
        try
        {
            State.CurrentState.RemoveLogin(property);
        }
        catch(Exception e)
        {
            Console.WriteLine("Failed to remove login: " + e.Message);
            Environment.Exit(1);
        }
    }

    private static void Secret(string clientPath)
    {
        try
        {
            Console.WriteLine(TextFileProcessor.GetKey(clientPath));
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Environment.Exit(1);
        }
        
    }
}