using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRLab.Mathematics;

namespace Jango
{
    public class Level1LogicPart : LogicPart
    {
        public override Settings GetDefaultSettings()
        {
            return new Settings {
                TimeLimit = double.PositiveInfinity,
                ObserverCameraLocation = new AIRLab.Mathematics.Frame3D(0,100,30,Angle.Zero,-Angle.HalfPi,Angle.Zero)
            
            };
        }

        public Level1LogicPart()
            : base(new JangoWorld(), () => new JangoKeyboardControllerPool())
        { }
    }
}
