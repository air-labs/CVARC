using AIRLab.Mathematics;
using CVARC.Basic.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.Basic
{
    public class SquareMovingBot : Bot
    {
        int state = 0;
        public override Controllers.Command MakeTurn()
        {
            state = (state + 1) % 8;
            if (state % 2 == 0)
                return new Command { RobotId=base.ControlledRobot, AngularVelocity = Angle.Pi / 2, Time = 1 };
            else
                return new Command { RobotId = base.ControlledRobot, LinearVelocity = 100, Time = 1 };
           
        }
    }
}












