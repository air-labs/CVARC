using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CVARC.V2
{
    public class SensorPack<TSensorData>
        where TSensorData : new()
    {
        List<ISensor> sensors = new List<ISensor>();

		public IEnumerable<ISensor> Sensors { get { return sensors; } }

        public Func<TSensorData> MeasureAll { get; private set; }
        public SensorPack(IActor actor)
        {
            var type = typeof(TSensorData);
            List<MemberBinding> bindings = new List<MemberBinding>();
            var measureMethod=typeof(ISensor).GetMethod("Measure");

            foreach (var property in type.GetProperties())
            {
                //initializing sensors
                var attributes = property.GetCustomAttributes(typeof(FromSensorAttribute), false);
                if (attributes.Length == 0)
                {
                    // throw new Exception("The type " + type.Name + " cannot be used as a sensor data, because no sensor specified for the property " + property.Name);
                    continue;
                }
                
                var attribute = attributes[0] as FromSensorAttribute;
                var sensorType = attribute.SensorType;
                var ctor = sensorType.GetConstructor(new Type[0]);
                if (ctor == null) throw new Exception("The type " + sensorType.Name + " cannot be used as a sensor, because it does not contain the parameterless constructor");
                var sensorObject = ctor.Invoke(new object[0]);
                if (!(sensorObject is ISensor))
                    throw new Exception("The type " + sensorType.Name + " cannot be used as a sensor, because it does not implement ISensor interface");
                var sensor = sensorObject as ISensor;
                if (sensor.MeasurementType!=property.PropertyType)
                    throw new Exception("The sensor "+sensorType.Name+" cannot be bound with property "+property.Name+" because their types do not match");
                sensor.Initialize(actor);
                sensors.Add(sensor);

                //creating bindings
                var binding =
                    Expression.Bind(property,
                        Expression.Convert(
                            Expression.Call(
                                Expression.Constant(sensor),
                                measureMethod),
                            property.PropertyType));
                bindings.Add(binding);
            }
            MeasureAll = Expression.Lambda<Func<TSensorData>>(
                Expression.MemberInit(
                    Expression.New(typeof(TSensorData).GetConstructor(new Type[0])),
                    bindings.ToArray())).Compile();
        }
    }
}
