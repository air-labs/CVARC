using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2.SimpleMovement;
using CVARC.V2;

namespace Demo
{
    public class CollisionRobot : SimpleMovementRobot<ICollisionActorManager, MovementWorld, SensorsData>
    {
        public string NearestObjectId;


        public override void ProcessCustomCommand(string commandName)
        {
            Manager.Grip(CollisionWorld.Objects[ControllerId]);
        }
     
    }
}
