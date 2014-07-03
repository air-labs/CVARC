using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Basic.Sensors;
using CVARC.Basic.Sensors.Positions;
using CVARC.Core;
using Gems.Sensors;
using StarshipRepair;
using System;
using System.Linq;

namespace Gems
{
    public class GemsRobot : Robot
    {
        private RobotIdSensor robotIdSensor;
        private MapSensor mapSensor;
        private LightHouseSensor lightHouseSensor;

        public GemsRobot(GemsWorld world, int number)
            : base(world, number)
        {
        }

        public override void Init()
        {
            robotIdSensor = new RobotIdSensor(this, World);
            mapSensor = new MapSensor(this, World);
            lightHouseSensor = new LightHouseSensor(this, World);
        }

        public override ISensorsData GetSensorsData()
        {
            return new SensorsData
                {
                    RobotIdSensor = robotIdSensor.Measure(),
                    LightHouseSensor = lightHouseSensor.Measure(),
                    MapSensor = mapSensor.Measure()
                };
        }



    }
}
