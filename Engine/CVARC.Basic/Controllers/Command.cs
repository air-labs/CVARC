using AIRLab.Mathematics;
using AIRLab.Thornado;

namespace CVARC.Basic.Controllers
{
    public class Command
    {
        [Thornado]
        public int RobotId { get; set; }
        [Thornado]
        public double Move { get; set; }
        [Thornado]
        public Angle Angle { get; set; }
        [Thornado]
        public string Cmd { get; set; }
    }
}