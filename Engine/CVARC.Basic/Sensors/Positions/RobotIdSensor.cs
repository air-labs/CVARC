using CVARC.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.Basic.Sensors
{

    public class RobotIdSensorData : ISensorData
    {
        public int Id { get; set; }


        public string GetStringRepresentation()
        {
            return "<YourRobotId>" + Id + "</YourRobotId>";
        }
    }


    public class RobotIdSensor : ISensor
    {
        int Id;
        public void Init(Robot robot, World wrld, DrawerFactory factory)
        {
            Id = robot.Number;
        }

        public ISensorData Measure()
        {
            return new RobotIdSensorData { Id = Id };
        }
    }
}
