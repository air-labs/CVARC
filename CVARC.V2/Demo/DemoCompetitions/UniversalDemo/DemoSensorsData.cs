using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using CVARC.V2;

namespace Demo
{
    [DataContract]
    public class DemoSensorsData
    {
        [DataMember]
        [FromSensor(typeof(LocatorSensor))]
        public LocatorItem[] Locations { get; set; }

		[DataMember]
		[FromSensor(typeof(GripSensor))]
		public bool IsGripped { get; set; }
        
    }
}
