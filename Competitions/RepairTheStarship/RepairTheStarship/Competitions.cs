using System.Drawing;
using AIRLab.Mathematics;
using CVARC.Basic;
using Gems.Bots;
using Gems.Robots;

namespace RepairTheStarship
{
    public abstract class SRCompetitions : Competitions
    {
        public const double MaxLinearVelocity = 50;
        public const double MaxAngularVelocity = 50;
        public static readonly Color WallColor = Color.LightGray;

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

        public SRCompetitions() : base( new GemsRules())
        {
            AvailableBots["Sanguine"] = typeof(Sanguine);
            AvailableBots["Vaermina"] = typeof(Vaermina);
            AvailableBots["MolagBal"] = typeof(MolagBal);
            AvailableBots["Azura"] = typeof(Azura);
        }
    }
}
