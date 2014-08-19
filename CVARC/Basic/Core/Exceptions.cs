using System;

namespace CVARC.Basic
{
    public class UserInputException : Exception
    {
        public UserInputException(Exception inner) : base(inner.Message) { }
    }
}
