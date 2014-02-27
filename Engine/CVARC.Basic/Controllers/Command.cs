using AIRLab.Mathematics;

namespace CVARC.Basic.Controllers
{
    public class Command
    {
        public int RobotId { get; set; }
        public double Move { get; set; }
        public Angle Angle { get; set; }
        public string Cmd { get; set; }
        public double Time { get; set; }

        public string Serialize()
        {
            return null;
        }
    }
}