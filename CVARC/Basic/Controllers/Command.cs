using AIRLab.Mathematics;
using CVARC.Basic.Core;

namespace CVARC.Basic.Controllers
{
    public class Command
    {
        public Angle Angle { get; set; }
        public int RobotId { get; set; }
        public double Move { get; set; }
        public CommandAction Action { get; set; }
        public double Time { get; set; }

        public static Command Mov(double distance)
        {
            return new Command {Move = distance, Time = 1};
        }

        public static Command Act(CommandAction action)
        {
            return new Command { Action = action, Time = 1 };
        }

        public static Command Rot(double grad)
        {
            return new Command { Angle = Angle.FromGrad(grad).Normilize(), Time = 1 }; 
        }

        public static Command Sleep(int time = 1)
        {
            return new Command { Time = time };
        }

        public override string ToString()
        {
            return string.Format("RobotId: {0} Move: {1} Angle: {2} Cmd: {3} Time: {4}", RobotId, Move, Angle, Action, Time);
        }
    }
}