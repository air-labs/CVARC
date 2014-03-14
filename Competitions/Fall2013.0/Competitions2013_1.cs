using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic;
using StarshipRepair.Bots;

namespace StarshipRepair
{
    public class SRCompetitions : Competitions
    {
        public const double MaxLinearVelocity = 50;
        public const double MaxAngularVelocity = 50;


        public override AIRLab.Mathematics.Angle AngularVelocityLimit
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
            : base(new GemsWorld(), new KbController(), new SRNetworkController())
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
