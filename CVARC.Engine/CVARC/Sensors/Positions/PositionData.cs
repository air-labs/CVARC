using System.Runtime.Serialization;
using AIRLab.Mathematics;

namespace CVARC.Basic.Sensors
{
    [DataContract]
    public class PositionData : ISensorData
    {
        [DataMember]
        public int RobotNumber;

        [DataMember]
        public double X { get; set; }

        [DataMember]
        public double Y { get; set; }

        [DataMember]
        public double Angle { get; set; }

        public PositionData(Frame3D position)
        {
            X = position.X;
            Y = position.Y;
            Angle = position.Yaw.Grad;
        }
    }
}