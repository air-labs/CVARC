using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRLab.Mathematics;
using CVARC.Core;
using CVARC.V2;

namespace Demo
{
    public class MovementWorldManager : KroRWorldManager<MovementWorld>
    {
        public override void CreateWorld(IdGenerator generator)
        {
            Root.Add(new Box
            {
                XSize = 200,
                YSize = 200,
                ZSize = 3,
                DefaultColor = Color.White,
                Top = new SolidColorBrush { Color = Color.Yellow },
                IsStatic = true,
                NewId = "floor",
            });
            Root.Add(new Box
            {
                XSize = 80,
                YSize = 5,
                ZSize = 10,
                DefaultColor = Color.Black,
                IsStatic = true,
                IsMaterial = true,
                Location = new Frame3D(0, 30, 0),
                NewId = "floor",
            });
            if (World.WorldState.objects)
            {
                Root.Add(new Box
                {
                    XSize = 15,
                    YSize = 15,
                    ZSize = 15,
                    DefaultColor = Color.Blue,
                    IsStatic = false,
                    IsMaterial = true,
                    NewId = generator.CreateNewId("Box1"),
                    Location = new Frame3D(50, 0, 0, Angle.Zero, Angle.Zero, Angle.Zero)
                });

            }


        }
    }
}
