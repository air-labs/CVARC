using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AIRLab.Mathematics;

namespace RoboMovies
{
    [DataContract]
    public class MapItem
    {
        [DataMember]
        public string Tag { get; set; }

        [DataMember]
        public double X { get; set; }

        [DataMember]
        public double Y { get; set; }

        public override string ToString()
        {
            return string.Format("Tag: {0} X:{1} Y:{2}", Tag, X, Y);
        }
    }
}
