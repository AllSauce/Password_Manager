global using System.Security.Cryptography;
global using System.Text;
global using System;
global using System.Collections.Generic;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var logins = new List<Login>();
logins.Add(new Login("test", "test", "test", "test"));
logins.Add(new Login("test2", "test2", "test2", "test2"));
logins.Add(new Login("test3", "test3", "test3", "test3"));
logins.Add(new Login("test4", "test4", "test4", "test4"));
logins.Add(new Login("test5", "test5", "test5", "test5"));

var key = KeyGenerator.Instance.GenerateKey();
var IV = new byte[16];
using (var generator = RandomNumberGenerator.Create())
{
    generator.GetBytes(IV);
}
var sucess = TextFileProcessor.Save(logins, Convert.FromBase64String(key), IV, "test");

Console.WriteLine("saving status " + sucess);

var loaded = TextFileProcessor.Load("test.txt", Convert.FromBase64String(key));

foreach (var login in logins)
{
    Console.WriteLine(login.Password);
}
Console.WriteLine();
Console.WriteLine(loaded.Success);

foreach (var login in loaded.logins)
{
    Console.WriteLine(login.Username);
}