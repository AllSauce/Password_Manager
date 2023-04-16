global using System.Security.Cryptography;
global using System.Text;
global using System;
global using System.Collections.Generic;

// See https://aka.ms/new-console-template for more information

class Program    
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Master Wayne.");
        try
        {
            CommandLineInterface.Command(args);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}