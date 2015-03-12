using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace CVARC.V2
{
    public class GripSensor : Sensor<bool,IGrippableRobot>
    {
        GripperUnit gripper;

        public override void Initialize(IActor actor)
        {
            base.Initialize(actor);
        }

        public override bool Measure()
        {
			return Actor.Gripper.GrippedObjectId != null;
        }
    }
}
