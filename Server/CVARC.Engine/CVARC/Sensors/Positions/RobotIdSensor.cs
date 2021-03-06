﻿using System.Runtime.Serialization;


namespace CVARC.Basic.Sensors
{
    [DataContract]
    public class RobotIdSensorData : ISensorData
    {
        [DataMember]
        public int Id { get; set; }
    }
    
    public class RobotIdSensor : Sensor<RobotIdSensorData>
    {
        int Id;

        public RobotIdSensor(Robot robot) : base(robot)
        {
            Id = robot.Number;
        }

        public override RobotIdSensorData Measure()
        {
            return new RobotIdSensorData { Id = Id };
        }
    }
}
