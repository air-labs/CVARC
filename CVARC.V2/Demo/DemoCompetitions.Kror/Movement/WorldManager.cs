﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                XSize = 100,
                YSize = 100,
                ZSize = 3,
                DefaultColor = Color.White,
                Top = new SolidColorBrush { Color = Color.Yellow },
                IsStatic = true,
                NewId = "floor",
            });
        }
    }
}