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
        public static Dictionary<string, string> Objects = new Dictionary<string, string> { { "Left", "ObjectLeft" }, { "Right", "ObjectRight" } };


        public override void Initialize(Competitions competitions, Configuration configuration, IControllerFactory controllerFactory)
        {
            base.Initialize(competitions, configuration, controllerFactory);
            var detector = new CollisionDetector(this);
            detector.FindControllableObject = side =>
            {
                var actor = Actors.OfType<CollisionRobot>().Where(z => z.ObjectId == side.ObjectId || Objects[z.ControllerId]==side.ObjectId).FirstOrDefault();
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
