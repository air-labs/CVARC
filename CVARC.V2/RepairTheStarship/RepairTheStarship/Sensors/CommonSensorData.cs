using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using CVARC.V2;

namespace RepairTheStarship
{
    [DataContract]
    public class CommonSensorData
    {
        [DataMember]
        [FromSensor(typeof(SelfLocationSensor))]
        public RTSLocatorItem SelfLocation { get; set; }

        [DataMember]
        [FromSensor(typeof(OpponentLocationSensor))]
        public RTSLocatorItem OpponentLocation { get; set; }

        [DataMember]
        [FromSensor(typeof(SelfIdSensor))]
        public string RobotId { get; set; }


        [DataMember]
        [FromSensor(typeof(GripSensor))]
        public bool HasGrippedDetail { get; set; }
    }
}
