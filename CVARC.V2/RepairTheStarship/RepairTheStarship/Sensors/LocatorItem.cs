using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CVARC.V2
{
    [DataContract]
    public class RTSLocatorItem
    {

        [DataMember]
        public double X { get; set; }

        [DataMember]
        public double Y { get; set; }

        [DataMember]
        public double Angle { get; set; }
    }
}
