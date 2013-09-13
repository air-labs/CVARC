using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic.Controllers;

namespace CVARC.Basic
{
    public abstract class FixedProgramBot : Bot
    {
        bool MirrorBot;
        List<Command> Program=new List<Command>();
        int iterator = 0;

        protected void Mov(double distance)
        {
            Program.Add(new Command
            {
                 Move = Math.Sign(distance)*Competitions.LinearVelocityLimit,
                 Time = Math.Abs(distance)/Competitions.LinearVelocityLimit,
                 RobotId=base.ControlledRobot
            });
        }

        protected void Rot(double angleGrad)
        {
            Program.Add(new Command
            {
                Angle=Math.Sign(angleGrad)*Competitions.AngularVelocityLimit*(MirrorBot?-1:1),
                Time=Math.Abs(angleGrad)/Competitions.AngularVelocityLimit.Grad,
                RobotId=base.ControlledRobot
            });
        }

        protected void Cmd(string cmd)
        {
            Program.Add(new Command
            {
                 Cmd=cmd,
                 RobotId=ControlledRobot,
                 Time=1
            });
        }

        public abstract void DefineProgram();

        override public void Initialize(Competitions competitions, int controlledRobot)
        {
            base.Initialize(competitions, controlledRobot);
            MirrorBot = controlledRobot != 0;
            DefineProgram();
        }

        public override Command MakeTurn()
        {
            if (iterator < Program.Count)
            {
                iterator++;
                return Program[iterator - 1];
            }
            else
            {
                return new Command
                {
                    Time = 1
                };
            }

        }
    }
}
