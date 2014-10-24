using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2.SimpleMovement;
using CVARC.V2;

namespace Demo
{
    public class MovementRobot : SimpleMovementRobot<IActorManager, MovementWorld, SensorsData>
    {

        public override void ProcessCustomCommand(string commandName, out double nextRequestTimeSpan)
        {
            nextRequestTimeSpan = 1;
        }

     
    }
}
