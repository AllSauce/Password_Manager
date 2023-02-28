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
                Command();
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

    private static void Init(string clientPath, string serverPath, string masterPassword)
    {
        State newVault = VaultFactory.CreateVault(clientPath ,masterPassword);
        //Måste spara encrypted vault och IV hos server.json eller skapa ny server fil?
        TextFileProcessor.SaveToServer(serverPath, newVault.Name, newVault.IV);
    }

    private static void Create()
    {

    }

    private static void Get()
    {

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
}