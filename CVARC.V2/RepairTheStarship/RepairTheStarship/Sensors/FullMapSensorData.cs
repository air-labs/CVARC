using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using CVARC.V2;

namespace RepairTheStarship
{
    [DataContract]
    public class FullMapSensorData : CommonSensorData
    {
        [DataMember]
        [FromSensor(typeof(MapSensor))]
        public Map Map { get; set; }

    }
}
