using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CVARC.V2
{
	[DataContract]
    public class CollisionSide
    {
		[DataMember]
        public string ObjectId;
		[DataMember]
		public string ControllerId;
		[DataMember]
		public string ControlledObjectId;
        public bool IsControllable { get { return ControllerId != null; } }
        public bool Match(CollisionSide side)
        {
            if (IsControllable != side.IsControllable) return false;
            if (IsControllable)
                return ControllerId == side.ControllerId;
            return ObjectId == side.ObjectId;
        }
    }

	[DataContract]
    public class CollisionCase
    {
		[DataMember]
        public CollisionSide Offender;
		[DataMember]
        public CollisionSide Victim;
    }

    public class CollisionDetector
    {
        IWorld world;
        public CollisionDetector(IWorld world)
        {
            this.world = world;
            world.Engine.Collision += Engine_Collision;
        }
        
        public Action<CollisionSide> FindControllableObject;
        public Action<CollisionCase> Account;
        public double CooldownTime = 0.001;

        List<CollisionCase> cases = new List<CollisionCase>();
        bool triggerSet = false;

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
            if (!triggerSet)
            {
                triggerSet = true;
                world.Clocks.AddTrigger(new OneTimeTrigger(world.Clocks.CurrentTime + CooldownTime, Resolve));
            }
        }


        void Resolve()
        {
            foreach (var e in cases)
                Account(e);
            cases.Clear();
            triggerSet = false;
        }


        public bool Guilty(CollisionCase c)
        {
            if (!c.Offender.IsControllable) return false;
            var vec = world.Engine.GetAbsoluteLocation(c.Victim.ObjectId) - world.Engine.GetAbsoluteLocation(c.Offender.ControlledObjectId);
            var velocity = world.Engine.GetSpeed(c.Offender.ControlledObjectId);
            return vec.X * velocity.X + vec.Y * velocity.Y > 0;
        }
    }
}
