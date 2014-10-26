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
    public class CameraWorldManager : KroRWorldManager<MovementWorld>
    {
        public override void CreateWorld(IdGenerator generator)
        {
            var colors = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow };
            var count =10;
            double a = 0;
            for (int i=0;i<count;i++)
            {
                Root.Add(new Cylinder
                {
                    Location = new AIRLab.Mathematics.Frame3D(100 * Math.Sin(a), 100 * Math.Cos(a), 0),
                    Height = 10,
                    RTop = 10,
                    RBottom = 10,
                    DefaultColor = colors[i%colors.Length]
                });
                a += Math.PI * 2.0 / count;
            }
        }
    }
}
