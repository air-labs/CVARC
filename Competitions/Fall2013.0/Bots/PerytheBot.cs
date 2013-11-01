using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipRepair.Bots
{
    public class PerytheBot : RunnedBot
    {
        public override void DefineProgram()
        {
            base.DefineProgram();
            var to = map.Nodes.OrderByDescending(a => a.Neighbors.Count).First();
            GoTo(to);
        }
    }
}
