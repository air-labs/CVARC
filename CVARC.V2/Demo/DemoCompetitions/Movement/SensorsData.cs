using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace Demo
{
    public class SensorsData
    {
        [FromSensor(typeof(LocatorSensor))]
        public LocatorItem[] Locations { get; set; }
    }
}
