using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Basic.Controllers;

namespace RepairTheStarship.Bots
{
    public class Sanguine : RunnedBot
    {
        public override void DefineProgram()
        {
            base.DefineProgram();
            Init(Competitions);
            var rand = new Random();
            int cnt = 0;
            while (cnt < 100)
            {
                var neigh = currentPosition.Neighbors;
                var next = neigh[rand.Next(neigh.Count)];
                if (next.X != currentPosition.X && next.Y != currentPosition.Y) continue;
                GoTo(next);
                cnt++;
            }
        }
    }
}
