using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RepairTheStarship
{
    [DataContract]
    public class DetailMapData
    {
        [DataMember]
        public DetailColor Color { get; set; }
        [DataMember]
        public PointF Location { get; set; }
    }
}
