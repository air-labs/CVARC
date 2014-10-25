using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.Core;
using CVARC.V2;

namespace Demo
{
    public class CameraWorldManager : KroRWorldManager<CameraWorld>
    {
        public override void CreateWorld(IdGenerator generator)
        {
            for (double a = 0; a < 2*Math.PI; a += Math.PI/3)
            {
                Root.Add(new Cylinder
                {
                    Location = new AIRLab.Mathematics.Frame3D(100 * Math.Sin(a), 100 * Math.Cos(a), 0),
                    Height = 10,
                    RTop = 10,
                    RBottom = 10,
                    DefaultColor = Color.Red
                });
            }
        }
    }
}
