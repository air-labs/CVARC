using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Core;

namespace RoboMovies
{
    public class Clapperboard : Box
    {
        public Box Cap { get; private set; }

        public Clapperboard(Frame3D location, Color color, string type)
        {
            XSize = 18;
            YSize = 5;
            ZSize = 18;
            DefaultColor = Color.Black;
            Location = location;
            IsStatic = true;
            NewId = type;

            Cap = BuildCap(color);
            Add(Cap);
        }
        
        private Box BuildCap(Color color)
        {
            return new Box
            {
                XSize = 4,
                YSize = 4,
                ZSize = 18,
                DefaultColor = color,
                Location = new Frame3D(8, 0, 16, Angle.Pi / 6, Angle.Zero, Angle.Zero),
                IsStatic = true,
                NewId = "cap",
            };
        }

        public void Close()
        {
            Remove(Cap);
            Cap = new Box
            {
                XSize = 18,
                YSize = 4,
                ZSize = 4,
                DefaultColor = Cap.DefaultColor,
                Location = new Frame3D(0, 0, 0),
                IsStatic = true,
                NewId = "CC",
            };
            Add(Cap);
        }
    }
}
