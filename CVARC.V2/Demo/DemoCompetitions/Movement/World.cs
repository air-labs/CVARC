using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace Demo
{
    public class MovementWorld : World<IWorldManager>, ISimpleMovementWorld
    {



        public double LinearVelocityLimit
        {
            get { return 200; }
        }

        public AIRLab.Mathematics.Angle AngularVelocityLimit
        {
            get { return Angle.FromGrad(200); }
        }

        public override void CreateWorld()
        {
           Manager.CreateWorld(IdGenerator);
        }


        public override void Initialize(Competitions competitions, Configuration configuration, ControllerFactory controllerFactory)
        {
            base.Initialize(competitions, configuration, controllerFactory);
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


        public static readonly SimpleMovementCommandHelper Helper = new SimpleMovementCommandHelper { LinearVelocityLimit=50, AngularVelocityLimit=Angle.FromGrad(90) };

        public SimpleMovementCommandHelper CommandHelper
        {
            get { return Helper; }
        }
    }
}
