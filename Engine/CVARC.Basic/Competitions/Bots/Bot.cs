using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic.Controllers;

namespace CVARC.Basic
{
    public abstract class Bot : Participant
    {
        public Competitions Competitions { get; private set; }
        public int ControlledRobot { get; private set; }
        public virtual void Initialize(Competitions competitions, int controllerRobot)
        {
            Competitions = competitions;
            ControlledRobot = controllerRobot;
        }
        
    }
}
