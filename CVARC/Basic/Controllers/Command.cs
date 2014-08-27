using AIRLab.Mathematics;
using CVARC.Basic.Core;
using System;

namespace CVARC.Basic.Controllers
{
    public class Command
    {
        public Angle AngularVelocity { get; set; }
        public int RobotId { get; set; }
        public double LinearVelocity { get; set; }
        public CommandAction Action { get; set; }
        public double Time { get; set; }
        const double MaxLinearVelocity = 50;
        const double MaxAngularVelocity = 90;

        public static Command Mov(double distance, double velocity = MaxLinearVelocity)
        {
            return new Command {
                LinearVelocity = Math.Sign(distance)*velocity, 
                Time = Math.Abs(distance/velocity)};
        }

        public static Command Act(CommandAction action)
        {
            return new Command { Action = action, Time = 1 };
        }

        public static Command Rot(double grad, double velocity=MaxAngularVelocity)
        {
            var rot= Angle.FromGrad(grad).Normilize();
            return new Command { AngularVelocity = 
                 Angle.FromGrad(MaxAngularVelocity*Math.Sign(rot.Grad)), 
                 Time = Math.Abs(rot.Grad/MaxAngularVelocity) }; 
        }

        public static Command Sleep(int time = 1)
        {
            return new Command { Time = time };
        }

        public override string ToString()
        {
            return string.Format("RobotId: {0} Move: {1} Angle: {2} Cmd: {3} Time: {4}", RobotId, LinearVelocity, AngularVelocity, Action, Time);
        }
    }
}