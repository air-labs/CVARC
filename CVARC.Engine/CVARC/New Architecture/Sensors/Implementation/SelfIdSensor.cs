using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class SelfIdSensor : Sensor<string,IControllable>
    {
        public override string Measure()
        {
            return Actor.ControllerId;
        }
    }
}
