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
            return string.Format("<RobotNumber>{0}</RobotNumber><Position>{1}</Position>", RobotNumber, new XML().WriteToString(Position));
        }
    }
}