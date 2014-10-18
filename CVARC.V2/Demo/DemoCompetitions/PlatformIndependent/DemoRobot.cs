using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2.SimpleMovement;
using CVARC.V2;

namespace DemoCompetitions
{
    public class DemoRobot : SimpleMovementRobot<IActorManager, DemoWorld, SensorsData>
    {

        public override void ProcessCustomCommand(string commandName, out double nextRequestTimeSpan)
        {
            nextRequestTimeSpan = 1;
        }

     
    }
}
