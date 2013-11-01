using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic.Controllers;

namespace StarshipRepair.Bots
{
    public class Vaermina : RunnedBot
    {
        public override Command MakeTurn()
        {
            var dst = Angem.Distance(
                Competitions.World.Robots[0].Body.Location.ToPoint3D(),
                Competitions.World.Robots[1].Body.Location.ToPoint3D())
                ;
            if (dst < 30) return new Command { Move = 0, Angle = Angle.FromGrad(0), Time = 1 };
            if (iterator >= Program.Count)
            {
                int posX =
                    (int)Math.Round((Competitions.World.Robots[1 - ControlledRobot].Body.GetAbsoluteLocation().X + 150) / 50);
                if (posX > 5) posX = 5;
                int posY =
                    (int) Math.Round((100-Competitions.World.Robots[1 - ControlledRobot].Body.GetAbsoluteLocation().Y)/50);
                if (posY > 3) posY = 3;
                var pth =
                    map.FindPath(currentPosition.X, currentPosition.Y, posX, posY)
                       .FirstOrDefault(a => a != currentPosition);
                if (pth != null)
                    GoTo(pth);
            }
            return base.MakeTurn();
        }
    }
}
