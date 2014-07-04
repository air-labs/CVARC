using System.Drawing;
using AIRLab.Mathematics;
using CVARC.Basic;
using Gems;
using StarshipRepair.Bots;

namespace StarshipRepair
{

    public class Level1 : SRCompetitions
    {
        public override ISceneSettings ParseSettings(CVARC.Network.HelloPackage helloPackage)
        {
            return SceneSettings.GetRandomMap(-1);
        }
    };

    public class Level2 : SRCompetitions
    {
        public override ISceneSettings ParseSettings(CVARC.Network.HelloPackage helloPackage)
        {
            return SceneSettings.GetRandomMap(helloPackage.MapSeed);
        }
    };


    public abstract class SRCompetitions : Competitions
    {
        public const double MaxLinearVelocity = 50;
        public const double MaxAngularVelocity = 50;


        public static string GripAction = "Grip";
        public static string ReleaseAction = "Release";

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

        public SRCompetitions() : base( new GemsRules(), new KbController())
        {
            AvailableBots["Simple"] = typeof(SimpleBot);
            AvailableBots["Sanguine"] = typeof(Sanguine);
            AvailableBots["Perythe"] = typeof(PerytheBot);
            AvailableBots["Vaermina"] = typeof(Vaermina);
            AvailableBots["MolagBal"] = typeof(MolagBal);
            AvailableBots["Namira"] = typeof(NamiraBot);
        }
    }
}
