using System;
using AIRLab.Mathematics;
using CVARC.Core;
using CVARC.Basic.Controllers;

namespace CVARC.Basic
{
    [Serializable]
    public abstract class Robot 
    {
        public string Name { get { return "R" + Number.ToString(); } }
        public readonly World World;
        public readonly int Number;
        
        public abstract ISensorsData GetSensorsData();
        public abstract void Init();

        protected Robot(World world, int number)
        {
            World = world;
            Number = number;
        }

        public void AddScore(int cnt, string msg = "")
        {
            World.Score.AddPenalty(new Penalty {Message = msg, RobotNumber = Number, Value = cnt});
        }

        public Frame3D GetAbsoluteLocation()
        {
            return World.Engine.GetAbsoluteLocation(Name);
        }

        public void ProcessCommand(Command cmd)
        {
            World.Engine.ProcessCommand(Name, cmd);
        }

    }
}
