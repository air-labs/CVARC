using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using CVARC.V2;

namespace Demo
{
    [DataContract]
    public class SensorsWithCamera
    {
        [DataMember]
        [FromSensor(typeof(CameraSensor))]
        public byte[] Image { get; set; }
    }
}
