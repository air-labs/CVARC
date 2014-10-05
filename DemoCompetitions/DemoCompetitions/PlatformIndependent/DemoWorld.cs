using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRLab.Mathematics;
using CVARC.V2;

namespace DemoCompetitions
{
    public class DemoWorld : World<object, IDemoWorldManagerPrototype>, ISimpleMovementWorld
    {
        DemoRobot robot1;
        DemoRobot robot2;

        protected override IEnumerable<IActor> CreateActors()
        {
            yield return robot1 = new DemoRobot(TwoPlayersId.Left);
            yield return robot2 = new DemoRobot(TwoPlayersId.Right);
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

    }
}
