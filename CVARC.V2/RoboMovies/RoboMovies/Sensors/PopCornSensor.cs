using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace RoboMovies
{
    public class PopCornSensor : Sensor<int, IRMRobot>
    {
        public override int Measure()
        {
            if (Actor.Gripper.GrippedObjectId != null)
                return (Actor.World as RMWorld).PopCornFullness[Actor.Gripper.GrippedObjectId];
            return 0;
        }
    }
}
