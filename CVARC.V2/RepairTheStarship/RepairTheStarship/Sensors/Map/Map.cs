using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RepairTheStarship
{
    [DataContract]
    public class Map
    {
        [DataMember]
        public DetailMapData[] Details { get; set; }

        [DataMember]
        public WallMapData[] Walls { get; set; }
    }
}
