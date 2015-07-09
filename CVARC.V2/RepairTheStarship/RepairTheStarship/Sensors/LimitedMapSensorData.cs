using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using CVARC.V2;

namespace RoboMovies
{
    [DataContract]
    public class LimitedMapSensorData : CommonSensorData
    {
        [DataMember]
        [FromSensor(typeof(MapSensorLimited))]
        public Map Map { get; set; }
    }
}
