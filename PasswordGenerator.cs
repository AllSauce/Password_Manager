public static class PasswordGenerator
{
    //Generates a password that is 20 characters long
    //using uppercase characters, lowercase characters and numbers
    public static string GeneratePassword()
    {
        string password = "";
        Random random = new Random();
        for (int i = 0; i < 20; i++)
        {
            int randomInt = random.Next(0, 3);
            switch (randomInt)
            {
                case 0:
                    password += (char)random.Next(65, 91);
                    break;
                case 1:
                    password += (char)random.Next(97, 123);
                    break;
                case 2:
                    password += random.Next(0, 10);
                    break;
            }
        }

        Console.WriteLine(password);

        return password;

        
    }
}
