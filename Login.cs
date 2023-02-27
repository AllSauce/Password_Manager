public struct Login
{
    public string Password { get; set; }
    public string Username { get; set; }
    public string Website { get; set; }
    public string Notes { get; set; }

    public Login(string username, string password, string website, string notes)
    {
        Username = username;
        Password = password;
        Website = website;
        Notes = notes;
    }

    public Login(string username, string password, string website) : this(username, password, website, "")
    {
        
    }
}
