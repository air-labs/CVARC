using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using CVARC.V2;

namespace RepairTheStarship
{
    [DataContract]
    public class RTSWorldState : IWorldState
    {
        [DataMember]
        public int Seed { get; set; }
    }
}
