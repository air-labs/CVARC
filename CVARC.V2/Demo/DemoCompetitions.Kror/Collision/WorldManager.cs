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
    public class CollisionWorldManager : MovementWorldManager
    {
        public override void CreateWorld(IdGenerator generator)
        {
            base.CreateWorld(generator);
            Root.Add(new Box
            {
                XSize = 10,
                YSize = 10,
                ZSize = 10,
                Location = new AIRLab.Mathematics.Frame3D(-50, 0, 0),
                DefaultColor = Color.Red,
                IsMaterial = true,
                NewId = CollisionWorld.Objects["Left"]
            });

            Root.Add(new Box
            {
                XSize = 10,
                YSize = 10,
                ZSize = 10,
                Location = new AIRLab.Mathematics.Frame3D(50, 0, 0),
                DefaultColor = Color.Red,
                IsMaterial = true,
                NewId = CollisionWorld.Objects["Right"]
            });
        }
    }
}
