using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2.SimpleMovement;
using CVARC.V2;

namespace Demo
{
    public class CollisionRobot : SimpleMovementRobot<IActorManager, MovementWorld, SensorsData>
    {
        public string NearestObjectId;


        public override void ProcessCustomCommand(string commandName)
        {
            var obj = World.IdGenerator.GetAllPairsOfType<GameObject>().Where(z => z.Item1.ControllerId == ControllerId).FirstOrDefault();
         //   Manager.Grip(obj.Item2);
        }
     
    }
}
