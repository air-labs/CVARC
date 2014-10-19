using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using CVARC.V2;

namespace RepairTheStarship
{
    [DataContract]
    public class Level3SensorData : Level1SensorData
    {
        [DataMember]
        [FromSensor(typeof(MapSensorLimited))]
        public MapItem[] Map { get; set; }
    }
}
