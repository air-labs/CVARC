using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace RoboMovies
{
    public class ScoreSensor : Sensor<int, IActor>
    {
        public override int Measure()
        {
            return Actor.World.Scores.GetTotalScore(Actor.ControllerId);
        }
    }
}
