using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.Basic
{
    public class UserInputException : Exception
    {
        public UserInputException(Exception inner) : base("User input exception", inner) { }

    }
}
