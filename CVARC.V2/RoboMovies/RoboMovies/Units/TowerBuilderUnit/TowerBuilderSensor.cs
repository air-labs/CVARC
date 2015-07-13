using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class TowerBuilderSensor : Sensor<int, ITowerBuilderRobot>
    {
        public override void Initialize(IActor actor)
        {
            base.Initialize(actor);
        }

        public override int Measure()
        {
			return Actor.TowerBuilder.CollectedCount;
        }
    }
}
