using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2.SimpleMovement;

namespace DemoCompetitions
{
    class DemoRobot : SimpleMovementRobot<DemoActorManager,DemoWorld,SensorsData>
    {
        public DemoRobot(string controllerId) : base(controllerId) { }

        public override void ProcessCustomCommand(string commandName, out double nextRequestTimeSpan)
        {
            if (commandName == "Make") Manager.MakeAction();
            nextRequestTimeSpan = 1;
        }

        protected override SensorsData GetSensorsData()
        {
            return new SensorsData();
        }
    }
}
