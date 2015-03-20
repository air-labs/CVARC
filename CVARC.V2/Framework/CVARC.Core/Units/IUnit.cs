using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface IUnit
    {
        UnitResponse ProcessCommand(object command);
    }
}
