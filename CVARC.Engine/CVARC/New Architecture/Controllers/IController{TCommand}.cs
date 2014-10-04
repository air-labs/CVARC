using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface IController<out TCommand> : IController
        where TCommand : ICommand
    {
        TCommand GetCommand();
    }
}
