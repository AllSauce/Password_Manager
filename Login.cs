public struct Login
{
    public string Password { get; set; }
    
    public string Property { get; set; }
    

    public Login(string property, string password)
    {
        this.Property = property;
        this.Password = password;
        
    }

    
}
