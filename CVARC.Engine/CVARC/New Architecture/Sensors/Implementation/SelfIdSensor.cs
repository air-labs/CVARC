using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class SelfIdSensor : Sensor<string,IActor>
    {
        public override string Measure()
        {
            return Actor.ControllerId;
        }
    }
}
