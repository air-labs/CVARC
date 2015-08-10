using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using CVARC.V2;

namespace Demo
{
    [DataContract]
    public class DWMSensorsData
    {
        [DataMember]
        [FromSensor(typeof(LocatorSensor))]
        public LocatorItem[] Locations { get; set; }

		[FromSensor(typeof(GAXSensor))]
		[DataMember]
		public List<GAXData> GAX { get; set; }

		[FromSensor(typeof(EncodersSensor))]
		[DataMember]
		public List<EncodersData> Encoders { get; set; }
    }
}
