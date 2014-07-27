using AIRLab.Mathematics;

namespace CVARC.Basic.Controllers
{
    public class Command
    {
        public int RobotId { get; set; }
        public double Move { get; set; }
        public Angle Angle { get; set; }
        public CommandAction Action { get; set; }
        public double Time { get; set; }

        public static Command Mov(double distance)
        {
            return new Command {Move = distance, Time = 1};
        }

        public static Command Rot(double angle)
        {
            return new Command { Angle = Angle.FromGrad(angle), Time = 1 }; 
        }

        public override string ToString()
        {
            return string.Format("RobotId: {0} Move: {1} Angle: {2} Cmd: {3} Time: {4}", RobotId, Move, Angle, Action, Time);
        }
    }
}