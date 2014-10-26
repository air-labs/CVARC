using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace Demo
{
    public class GameObject
    {
        public string ControllerId;
    }

    public class CollisionWorld : MovementWorld
    {
        public override void Initialize(Competitions competitions, IRunMode environment)
        {
            base.Initialize(competitions, environment);
            var detector = new CollisionDetector(this);
            detector.FindControllableObject = side =>
            {
                var actor = Actors.OfType<MovementRobot>().Where(z => z.ObjectId == side.ObjectId).FirstOrDefault();
                if (actor != null)
                {
                    side.ControlledObjectId = actor.ObjectId;
                    side.ControllerId = actor.ControllerId;
                }
            };

            detector.Account = c =>
            {
                if (!c.Victim.IsControllable) return;
                if (!detector.Guilty(c)) return;
                Scores.Add(c.Offender.ControllerId, -1, "Collision");
            };
        }
    }
}
