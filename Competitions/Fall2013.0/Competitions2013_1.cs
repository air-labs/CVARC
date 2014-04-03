using AIRLab.Mathematics;
using CVARC.Basic;
using StarshipRepair.Bots;

namespace StarshipRepair
{
    public class SRCompetitions : Competitions
    {
        public const double MaxLinearVelocity = 50;
        public const double MaxAngularVelocity = 50;

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

        public SRCompetitions() : base(new GemsWorld(), new KbController())
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
