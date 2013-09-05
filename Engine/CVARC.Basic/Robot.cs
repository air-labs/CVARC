using System;
using System.Collections.Generic;
using AIRLab.Mathematics;
using CVARC.Basic.Sensors;
using CVARC.Core;

namespace CVARC.Basic
{
    [Serializable]
    public class Robot
    {
        private readonly World _world;
        public int Number { get; set; }
        public Body Body { get; set; }
        [NonSerialized]
        public List<Sensor> Sensors = new List<Sensor>();

        public Frame3D RequestedSpeed { get; set; }

        public Robot(World world)
        {
            _world = world;
        }
        public void AddScore(int cnt, string msg = "")
        {
            _world.Score.AddPenalty(new Penalty{Message = msg, RobotNumber = Number, Value = cnt});
        }

        public void SetVelocity()
        {
            Body.Velocity = RequestedSpeed;
        }
    }
}
