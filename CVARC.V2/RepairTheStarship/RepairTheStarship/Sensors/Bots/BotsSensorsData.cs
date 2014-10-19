using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using CVARC.V2;

namespace RepairTheStarship
{
    public class BotsSensorsData
    {
        [DataMember]
        [FromSensor(typeof(LocatorSensor))]
        public LocatorItem[] RobotsLocations { get; set; }

        [DataMember]
        [FromSensor(typeof(SelfIdSensor))]
        public string RobotId { get; set; }

        [DataMember]
        [FromSensor(typeof(InternalMapSensor))]
        public MapItem[] Map { get; set; }

        [DataMember]
        [FromSensor(typeof(GripSensor))]
        public bool HasGrippedDetail { get; set; }
    }
}
