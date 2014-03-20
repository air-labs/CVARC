using System;
using AIRLab.Mathematics;
using CVARC.Core;
using CVARC.Basic.Controllers;

namespace CVARC.Basic
{
    [Serializable]
    public abstract class Robot
    {
        protected readonly World World;
        public int Number { get; set; }
        public Body Body { get; set; }
        public abstract ISensorsData GetSensorsData();
        public abstract void ProcessCommand(Command cmd);
        public abstract void Init();
        protected Frame3D RequestedSpeed { get; set; }

        protected Robot(World world)
        {
            World = world;
        }

        public void AddScore(int cnt, string msg = "")
        {
            World.Score.AddPenalty(new Penalty {Message = msg, RobotNumber = Number, Value = cnt});
        }

        public void SetVelocity()
        {
            Body.Velocity = RequestedSpeed;
        }
    }
}
