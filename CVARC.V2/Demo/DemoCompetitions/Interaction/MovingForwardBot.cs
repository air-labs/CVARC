using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace Demo
{
    public class MovingForwardBot : Controller<SimpleMovementCommand>
    {
        public override SimpleMovementCommand GetCommand()
        {
            return SimpleMovementCommand.Move(50, 1);
        }
    }
}
