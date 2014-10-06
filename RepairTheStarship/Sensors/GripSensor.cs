using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace RepairTheStarship
{
    public class GripSensor : Sensor<bool,IRTSRobot>
    {
        public override bool Measure()
        {
            return Actor.GrippedObjectId != null;
        }
    }
}
