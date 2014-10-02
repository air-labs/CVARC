using System;
using AIRLab.Mathematics;
using CVARC.Basic.Controllers;

namespace CVARC.Basic
{
    [Serializable]
    public abstract class Robot
    {
        public string Name { get { return (Number + 1).ToString(); } }
        public readonly Competitions Competitions;
        public readonly int Number;
        
        public abstract T GetSensorsData<T>() where T : class , ISensorsData;
        public abstract void Init();

        protected Robot(Competitions competitions, int number)
        {
            Competitions = competitions;
            Number = number;
        }

        public Frame3D GetAbsoluteLocation()
        {
            return Competitions.Engine.GetAbsoluteLocation(Name);
        }

        public void ProcessCommand(Command cmd)
        {
            Competitions.Engine.ProcessCommand(Name, cmd);
        }
    }
}
