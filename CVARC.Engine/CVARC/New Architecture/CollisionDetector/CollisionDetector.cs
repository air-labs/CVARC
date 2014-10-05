using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{

    public class CollisionSide
    {
        public string ObjectId;
        public string ControllerId;
        public string ControlledObjectId;
        public bool IsControllable { get { return ControllerId == null; } }
        public bool Match(CollisionSide side)
        {
            if (IsControllable != side.IsControllable) return false;
            if (IsControllable)
                return ControllerId == side.ControllerId;
            return ObjectId == side.ObjectId;
        }
    }

    public class CollisionCase
    {
        public CollisionSide Offender;
        public CollisionSide Victim;
    }

    public abstract class CollisionDetector
    {
        IWorld world;
        public CollisionDetector(IWorld world)
        {
            this.world = world;
            world.Engine.Collision += Engine_Collision;
        }
        
        public Action<CollisionSide> FindControllableObject;

        List<CollisionCase> cases = new List<CollisionCase>();

        void Engine_Collision(string arg1, string arg2)
        {
            var side1 = new CollisionSide { ObjectId = arg1 };
            var side2 = new CollisionSide { ObjectId = arg2 };
            FindControllableObject(side1);
            FindControllableObject(side2);

            if (cases.Any(z => z.Offender.Match(side1) && z.Victim.Match(side2)))
                return;
            cases.Add(new CollisionCase { Offender = side1, Victim = side2 });
            cases.Add(new CollisionCase { Offender = side2, Victim = side1 });
        }
    }
}
