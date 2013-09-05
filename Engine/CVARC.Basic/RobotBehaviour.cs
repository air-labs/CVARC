using System;
using System.Collections.Generic;
using AIRLab.Mathematics;
using CVARC.Basic.Controllers;
using CVARC.Basic.Sensors;

namespace CVARC.Basic
{
    public class RobotBehaviour
    {
        public event Action<Robot, Command> CommandRecieved = (robot, d) => { };
        public List<ISensorFactory> Sensors { get; set; }
        public void Add<T>() where T : ISensor, new()
        {
            Sensors.Add(new SensorFactory<T>());
        }
        public RobotBehaviour()
        {
            Sensors = new List<ISensorFactory>();
        }

        public void ProcessCommand(Robot robot, Command cmd)
        {
            if (Math.Abs(cmd.Move) > 0 || Math.Abs(cmd.Angle.Grad) > 0)
                robot.RequestedSpeed = new Frame3D(cmd.Move * Math.Cos(robot.Body.Location.Yaw.Radian), cmd.Move * Math.Sin(robot.Body.Location.Yaw.Radian), 0, Angle.Zero, cmd.Angle, Angle.Zero);
            CommandRecieved(robot, cmd);
        }

        public virtual void InitSensors()
        {
        }
    }
}