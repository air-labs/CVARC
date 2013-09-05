using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic.Controllers;

namespace CVARC.Basic
{
    public interface IParticipant
    {
        Command MakeTurn();
    }
}
