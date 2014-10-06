using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2.SimpleMovement;
using RepairTheStarship.Robot;

namespace RepairTheStarship
{
    public class RTSWorld : World<SceneSettings,IRTSWorldManager>, ISimpleMovementWorld
    {
        Dictionary<DetailColor, int> repairs = new Dictionary<DetailColor, int>();

        public RTSWorld()
        {
            repairs[DetailColor.Blue] = 0;
            repairs[DetailColor.Green] = 0;
            repairs[DetailColor.Red] = 0;
        }

        public string GripCommand = "Grip";
        public string ReleaseCommand = "Release";

        public override void Initialize(Competitions competitions, IRunMode environment)
        {
            base.Initialize(competitions, environment);
            var detector = new CollisionDetector(this);
            detector.FindControllableObject = side =>
                {
                    var robot = Actors
                        .OfType<IRTSRobot>()
                        .Where(z => z.ObjectId == side.ObjectId || z.GrippedObjectId == side.ObjectId)
                        .FirstOrDefault();
                    if (robot != null)
                    {
                        side.ControlledObjectId = robot.ObjectId;
                        side.ControllerId = robot.ControllerId;
                    }
                };
            detector.Account = c =>
                {
                    if (!c.Victim.IsControllable) return;
                    if (!detector.Guilty(c)) return;
                    Scores.Add(c.Offender.ControllerId, -30, "Collision");
                };
        }


        public void DetailInstalled(DetailColor color, string robotId)
        {
            Scores.Add(robotId, 10, "Repaired " + color);
            repairs[color]++;
            if (repairs[color] == 2)
            {
                Scores.Add(TwoPlayersId.Left, 5, "Cooperative action for color " + color);
                Scores.Add(TwoPlayersId.Right, 5, "Cooperative action for color " + color);
            }
            if (repairs.Values.Sum() == 6)
            {
                Scores.Add(TwoPlayersId.Left, 5, "Cooperative action for all colors");
                Scores.Add(TwoPlayersId.Right, 5, "Cooperative action for all colors");
            }
        }

        protected override IEnumerable<IActor> CreateActors()
        {
            yield return new Level1Robot(TwoPlayersId.Left);
            yield return new Level1Robot(TwoPlayersId.Right);
        }

        public double LinearVelocityLimit
        {
            get { return 50; }
        }

        public AIRLab.Mathematics.Angle AngularVelocityLimit
        {
            get { return Angle.FromGrad(90); }
        }
    }
}
