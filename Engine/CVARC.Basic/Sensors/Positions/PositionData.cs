using AIRLab.Mathematics;
using AIRLab.Thornado;

namespace CVARC.Basic.Sensors
{
    public class PositionData : ISensorData
    {
        public int RobotNumber;
        public Frame3D Position;
        public string GetStringRepresentation()
        {
            return string.Format("<Robot><Number>{0}</Number><X>{1}</X><Y>{2}</Y><Angle>{3}</Angle></Robot>", RobotNumber, Position.X, Position.Y, Position.Yaw.Grad);
        }
    }
}