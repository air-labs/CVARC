using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Core;
using RepairTheStarship.Bots;
using RepairTheStarship.Robots;

namespace RepairTheStarship
{
    public abstract class SRCompetitions : Competitions
    {
        public const double MaxLinearVelocity = 50;
        public const double MaxAngularVelocity = 50;
        public static readonly Color WallColor = Color.LightGray;
        private DateTime oldCollisionTime = DateTime.Now;
        private Dictionary<CollisionType, int> countRepairedWalls = new Dictionary<CollisionType, int>
        {
            {CollisionType.BlueWallRepaired, 0},
            {CollisionType.GreenWallRepaired, 0},
            {CollisionType.RedWallRepaired, 0}
        }; 

        public override int RobotCount
        {
            get { return 2; }
        }
        public override int CompetitionId { get { return 5; } }

        public override Robot CreateRobot(int robotNumber)
        {
            return new GemsRobot(this, robotNumber);
        }

        public override Angle AngularVelocityLimit
        {
            get
            {
                return Angle.FromGrad(MaxAngularVelocity);
            }
        }

        public override double LinearVelocityLimit
        {
            get
            {
                return MaxLinearVelocity;
            }
        }

        public SRCompetitions()
        {
            AvailableBots["Sanguine"] = typeof(Sanguine);
            AvailableBots["MolagBal"] = typeof(MolagBal);
            AvailableBots["Azura"] = typeof(Azura);
            AvailableBots["Vaermina"] = typeof(Vaermina);
        }

        public override void Initialize(IEngine engine, RobotSettings[] robotSettings)
        {
            base.Initialize(engine, robotSettings);
            Engine.OnCollision += EngineOnCollision;
        }

        private void AddScore(string message, int robotNumber, int points)
        {
            Score.AddPenalty(new Penalty { Message = message, RobotNumber = robotNumber, Value = points });
        }

        private void EngineOnCollision(OnCollisionEventHandlerArgs args)
        {
            int robotNumber = int.Parse(args.FirstBodyId) - 1;
            switch (args.CollisionType)
            {
                case CollisionType.RobotCollision:
                    bool isRobot = args.SecondBodyId == "1" || args.SecondBodyId == "2";
                    if (isRobot && (DateTime.Now - oldCollisionTime).TotalMilliseconds > 1000 && IsFault(args.FirstBodyId, args.SecondBodyId))
                    {
                        oldCollisionTime = DateTime.Now;
                        AddScore("RobotCollision", robotNumber, -30);
                    }
                    break;
                case CollisionType.BlueWallRepaired:
                case CollisionType.GreenWallRepaired:
                case CollisionType.RedWallRepaired:
                    countRepairedWalls[args.CollisionType]++;
                    if (countRepairedWalls[args.CollisionType] == 2)
                    {
                        AddRobotsPoints("All" + args.CollisionType, 5);
                        if (countRepairedWalls.All(x => x.Value == 2))
                            AddRobotsPoints("AllWallsRepaired", 5);
                    }
                    AddScore("WallRepaired", robotNumber, 10);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(args.CollisionType.ToString());
            }
        }

        private void AddRobotsPoints(string text, int points)
        {
            AddScore(text, 0, points);
            AddScore(text, 1, points);
        }

        private bool IsFault(string firstRobotId, string secondRobotId)
        {
            var vec = Engine.GetAbsoluteLocation(secondRobotId) - Engine.GetAbsoluteLocation(firstRobotId);
            var velocity = Engine.GetSpeed(firstRobotId);
            return vec.X*velocity.X + vec.Y*velocity.Y > 0;
        }
    }
}
