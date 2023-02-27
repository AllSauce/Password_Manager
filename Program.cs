global using System.Security.Cryptography;
global using System.Text;
global using System;
global using System.Collections.Generic;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");








VaultFactory.LoadVault("GigaVault", Console.ReadLine());

if(State.CurrentState.Success == false)
{
    Console.WriteLine("Failed to load vault");
    return;
}
else if(State.CurrentState.Success == true)
{
    Console.WriteLine("Vault loaded successfully");
}

foreach(var login in State.CurrentState.Passwords)
{
    Console.WriteLine(login.Value.Website);
}
