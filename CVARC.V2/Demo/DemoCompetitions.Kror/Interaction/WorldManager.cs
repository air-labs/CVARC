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
    public class InteractionWorldManager : KroRWorldManager<MovementWorld>
    {
        public override void CreateWorld(IdGenerator generator)
        {
            Root.Add(new Box
            {
                XSize = 100,
                YSize = 10,
                ZSize = 1,
                DefaultColor = Color.Black,
                IsStatic = true,
                IsMaterial = true,
                NewId = "border",
                Location = new Frame3D(10, 25, 0, Angle.Zero, Angle.Zero, Angle.Zero)
            });
            Root.Add(new Box
            {
                XSize = 15,
                YSize = 15,
                ZSize = 15,
                DefaultColor = Color.Blue,
                IsStatic = false,
                IsMaterial = true,
                NewId = generator.CreateNewId("Box1"),
                Location = new Frame3D(0, 35, 0, Angle.Zero, Angle.Zero, Angle.Zero),
                FrictionCoefficient = 8
            });
            Root.Add(new Box
            {
                XSize = 15,
                YSize = 15,
                ZSize = 15,
                DefaultColor = Color.Blue,
                IsStatic = false,
                IsMaterial = true,
                NewId = generator.CreateNewId("Box2"),
                Location = new Frame3D(-25, 0, 0, Angle.Zero, Angle.Zero, Angle.Zero),
                FrictionCoefficient = 8
            });
            Root.Add(new Box
            {
                XSize = 15,
                YSize = 15,
                ZSize = 15,
                DefaultColor = Color.Blue,
                IsStatic = false,
                IsMaterial = true,
                NewId = generator.CreateNewId("Box3"),
                Location = new Frame3D(0, -25, 0, Angle.Zero, Angle.Zero, Angle.Zero),
                FrictionCoefficient = 8
            });
            Root.Add(new Box
            {
                XSize = 150,
                YSize = 150,
                ZSize = 1,
                DefaultColor = Color.Yellow,
                IsStatic = true,
                IsMaterial = false,
                NewId = "floor",
                Location = new Frame3D(0, 0, -1, Angle.Zero, Angle.Zero, Angle.Zero)
            });
            Root.Add(new Box
            {
                XSize = 150,
                YSize = 2,
                ZSize = 5,
                DefaultColor = Color.Black,
                IsStatic = true,
                IsMaterial = true,
                NewId = "border",
                Location = new Frame3D(0, -75, 0, Angle.Zero, Angle.Zero, Angle.Zero)
            });
            Root.Add(new Box
            {
                XSize = 150,
                YSize = 2,
                ZSize = 5,
                DefaultColor = Color.Black,
                IsStatic = true,
                IsMaterial = true,
                NewId = "border",
                Location = new Frame3D(0, 75, 0, Angle.Zero, Angle.Zero, Angle.Zero)
            });
            Root.Add(new Box
            {
                XSize = 2,
                YSize = 150,
                ZSize = 5,
                DefaultColor = Color.Black,
                IsStatic = true,
                IsMaterial = true,
                NewId = "border",
                Location = new Frame3D(-75, 0, 0, Angle.Zero, Angle.Zero, Angle.Zero)
            });
            Root.Add(new Box
            {
                XSize = 2,
                YSize = 150,
                ZSize = 5,
                DefaultColor = Color.Black,
                IsStatic = true,
                IsMaterial = true,
                NewId = "border",
                Location = new Frame3D(75, 0, 0, Angle.Zero, Angle.Zero, Angle.Zero)
            });

        }
    }
}
