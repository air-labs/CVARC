using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface IUnit
    {
    }

    public interface IUnit<in TCommand> : IUnit
    {
        UnitResponse ProcessCommand(TCommand command);
    }
}
