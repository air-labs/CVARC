using System;

namespace CVARC.Basic
{
    public class UserInputException : Exception
    {
        public UserInputException(Exception inner) : base("User input exception", inner) { }
        public UserInputException(string message) : this(new Exception(message)) { }
    }
}
