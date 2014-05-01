using AIRLab.Mathematics;

namespace CVARC.Basic.Controllers
{
    public class Command
    {
        public int RobotId { get; set; }
        public double Move { get; set; }
        public Angle Angle { get; set; }
        public Cmd? Cmd { get; set; }
        public double Time { get; set; }

        public override string ToString()
        {
            return string.Format("RobotId: {0} Move: {1} Angle: {2} Cmd: {3} Time: {4}", RobotId, Move, Angle, Cmd, Time);
        }
    }
}