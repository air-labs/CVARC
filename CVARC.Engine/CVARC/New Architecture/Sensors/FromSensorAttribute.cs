using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class FromSensorAttribute : Attribute
    {
        public Type SensorType { get; private set; }
        public FromSensorAttribute(Type sensorType)
        {
            SensorType = sensorType;
        }
    }
}
