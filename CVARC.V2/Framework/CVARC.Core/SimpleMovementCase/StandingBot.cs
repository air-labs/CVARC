using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;

namespace CVARC.V2.SimpleMovement
{
    public class StandingBot : Controller<SimpleMovementCommand>
    {
        
        override public SimpleMovementCommand GetCommand()
        {
            return SimpleMovementCommand.Exit();
        }

    }
}
