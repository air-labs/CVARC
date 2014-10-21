using System;
using System.Linq;
using AIRLab.Mathematics;
using CVARC.Basic;

namespace RepairTheStarship.Sensors
{
    public class DestinationMapSensor : MapSensor
    {
        private const int Destination = 100;

        public DestinationMapSensor(Robot robot) : base(robot)
        {
        }

        public override MapSensorData Measure()
        {
            var data = base.Measure();
            var location = Robot.GetAbsoluteLocation();
            data.MapItems = data.MapItems.Where(x => GetDestination(location, x.X, x.Y) < Destination).ToArray();
            return data;
        }

        private double GetDestination(Frame3D robotLocation, double itemX, double itemY)
        {
            return Math.Abs(robotLocation.X - itemX) + Math.Abs(robotLocation.Y - itemY); 
        }
    }
}