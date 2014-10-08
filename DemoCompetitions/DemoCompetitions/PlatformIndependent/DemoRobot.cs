using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2.SimpleMovement;

namespace DemoCompetitions
{
    public class DemoRobot : SimpleMovementRobot<IDemoActorManagerPrototype, DemoWorld, SensorsData>
    {
        public DemoRobot(string controllerId) : base(controllerId) { }

        public override void ProcessCustomCommand(string commandName, out double nextRequestTimeSpan)
        {
            if (commandName == "Make") Manager.MakeAction();
            nextRequestTimeSpan = 1;
        }

     
    }
}
