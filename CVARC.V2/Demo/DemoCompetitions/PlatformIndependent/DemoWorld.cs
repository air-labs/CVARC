using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace DemoCompetitions
{
    public class DemoWorld : World<SceneSettings, IWorldManager>, ISimpleMovementWorld
    {
        DemoRobot robot1;
        DemoRobot robot2;

        public override IEnumerable<string> ControllersId
        {
            get
            {
                yield return TwoPlayersId.Left;
                yield return TwoPlayersId.Right;
            }
        }

        public override IActor CreateActor(string controllerId)
        {
            return new DemoRobot();
        }
        public override SceneSettings CreateSceneState(int seed)
        {
            return new SceneSettings();
        }

        public double LinearVelocityLimit
        {
            get { return 200; }
        }

        public AIRLab.Mathematics.Angle AngularVelocityLimit
        {
            get { return Angle.FromGrad(200); }
        }



        public override void Initialize(Competitions competitions, IRunMode environment)
        {
            base.Initialize(competitions, environment);
            var detector = new CollisionDetector(this);
            detector.FindControllableObject = side =>
                {
                    var actor = Actors.OfType<DemoRobot>().Where(z => z.ObjectId == side.ObjectId).FirstOrDefault();
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
