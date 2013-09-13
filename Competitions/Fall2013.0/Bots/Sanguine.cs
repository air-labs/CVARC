using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Basic.Controllers;

namespace StarshipRepair.Bots
{
    public class Sanguine : Bot
    {
        Random rnd = new Random();
        double MoveTime = 50 / SRCompetitions.MaxLinearVelocity;
        double RotTime = 90 / SRCompetitions.MaxAngularVelocity;

        public override CVARC.Basic.Controllers.Command MakeTurn()
        {
            var direction = rnd.Next(100) < 50 ? -1 : 1;
            if (rnd.Next(100) < 50)
                return new Command { Move = SRCompetitions.MaxLinearVelocity*direction, Time = MoveTime };
            else
                return new Command { Angle = Angle.FromGrad(SRCompetitions.MaxAngularVelocity * direction), Time = RotTime };
        }
    }
}
