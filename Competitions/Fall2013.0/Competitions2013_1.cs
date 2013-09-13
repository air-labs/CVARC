using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;
using StarshipRepair.Bots;

namespace StarshipRepair
{
    public class SRCompetitions : Competitions
    {
        public const double MaxLinearVelocity = 30;
        public const double MaxAngularVelocity = 30;


        public SRCompetitions()
            : base(new GemsWorld(), new Behaviour(), new KbController(), new SRNetworkController())
        {
            AvailableBots["Simple"] = typeof(SimpleBot);
            AvailableBots["Sanguine"] = typeof(Sanguine);
        }
    }
}
