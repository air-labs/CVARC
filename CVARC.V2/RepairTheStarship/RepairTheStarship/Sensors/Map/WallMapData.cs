using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RepairTheStarship
{
    [DataContract]
    public class WallMapData
    {
        [DataMember]
        public PointF FirstEnd { get; set; }
        [DataMember]
        public PointF SecondEnd { get; set; }
        [DataMember]
        public PointF Center { get; set; }
        [DataMember]
        public WallOrientation Orientation { get; set; }
        [DataMember]
        public WallSettings Type { get; set; }
    }
}
