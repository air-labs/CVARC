using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic.Controllers;

namespace CVARC.Basic
{
    public abstract class FixedProgramBot : Bot
    {
        bool MirrorBot;
        protected List<Command> Program=new List<Command>();
        protected int iterator = 0;

        protected void Mov(double distance)
        {
            MulAdd(new Command
            {
                 Move = Math.Sign(distance)*Competitions.LinearVelocityLimit,
                 Time = Math.Abs(distance)/Competitions.LinearVelocityLimit,
                
            });
        }

        protected void Rot(double angleGrad)
        {
            if (angleGrad == 0) return;
            MulAdd(new Command
            {
                Angle=Math.Sign(angleGrad)*Competitions.AngularVelocityLimit*(MirrorBot?-1:1),
                Time=Math.Abs(angleGrad)/Competitions.AngularVelocityLimit.Grad,
               
            });
        }

        void MulAdd(Command cmd)
        {
            int parts = 10;
            for (int i = 0; i < parts; i++)
                Program.Add(new Command { Move = cmd.Move, Angle = cmd.Angle, Time = cmd.Time / parts });
        }

        protected void Command(Cmd cmd)
        {
            Program.Add(new Command
            {
                 Action=cmd.ToString(),
                 Time=1
            });
        }

        public abstract void DefineProgram();

        override public void Initialize(Competitions competitions)
        {
            base.Initialize(competitions);
            MirrorBot = ControlledRobot != 0;
            DefineProgram();
        }

        public override Command MakeTurn()
        {
            var dst=Angem.Distance(
                Competitions.World.Robots[0].GetAbsoluteLocation().ToPoint3D(),
                Competitions.World.Robots[1].GetAbsoluteLocation().ToPoint3D())
                ;
            if ( dst   < 30) return new Command { Move=0, Angle=Angle.FromGrad(0), Time = 1 };  


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
