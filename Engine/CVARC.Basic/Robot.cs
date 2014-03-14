using System;
using System.Collections.Generic;
using AIRLab.Mathematics;
using CVARC.Basic.Sensors;
using CVARC.Core;
using CVARC.Basic.Controllers;

namespace CVARC.Basic
{
    [Serializable]
    public abstract class Robot
    {
        private readonly World _world;
        public int Number { get; set; }
        public Body Body { get; set; }


        public abstract object GetSensorsData();
        public abstract void ProcessCommand(Command cmd);

//        [NonSerialized] public List<Sensor> Sensors = new List<Sensor>();

        public Frame3D RequestedSpeed { get; set; }

        public Robot(World world)
        {
            _world = world;
        }

        public void AddScore(int cnt, string msg = "")
        {
            _world.Score.AddPenalty(new Penalty {Message = msg, RobotNumber = Number, Value = cnt});
        }

        public void SetVelocity()
        {
            Body.Velocity = RequestedSpeed;
        }

        //public SensorsData GetSensorsData()
        //{
        //    var data = new SensorsData();
        //    //TODO Сделать нормально
        //    foreach (var sensor in Sensors)
        //    {
        //        switch (sensor.Name)
        //        {
        //            case "MapSensor":
        //                data.MapSensor = sensor.Measure<MapSensorData>();
        //                break;
        //            case "LightHouseSensor":
        //                data.LightHouseSensor = sensor.Measure<ManyPositionData>();
        //                break;
        //            case "RobotIdSensor":
        //                data.RobotIdSensor = sensor.Measure<RobotIdSensorData>();
        //                break;
        //            default:
        //                throw new Exception("Unknown sensorData");
        //        }
        //    }
        //    return data;
        //}
    }
}
