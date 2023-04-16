[Serializable]
public class NoInputException : Exception
{
    public NoInputException() { }

    public NoInputException(string parameter)
        : base(String.Format("There was no input on the following: {0}", parameter))
    {

    }

    public NoInputException(string parameter1, string parameter2)
        : base(String.Format("There was no input on one of the following: {0} and {1}", parameter1, parameter2))
    {

    }
}

[Serializable]
public class WrongInputException : Exception
{
    public WrongInputException() {    }

    public WrongInputException(string parameter)
        : base(String.Format("Incorrect input for: {0}", parameter))
    {

    }

    public WrongInputException(string parameter1, string parameter2)
        : base(String.Format("One of the following was incorrect: {0} and {1}", parameter1, parameter2))
    {

    }
}

[Serializable]
public class WrongPasswordException : Exception
{
    public WrongPasswordException() {      }

    public WrongPasswordException(string parameter)
        :   base(String.Format("The password: '{0}' is incorrect! Please try again.", parameter))
    {
        
    }


}