using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Core;

namespace Gems
{
    public class GemsWorld : World
    {
        public override int RobotCount
        {
            get { return 1; }
        }
        public override Body CreateWorld(IEnumerable<Robot> robots)
        {
            var root = new Body();
            var enumerable = robots as IList<Robot> ?? robots.ToList();
            var first = enumerable[0];
            first.Body = new Box(20, 20, 20)
            {
                Location = new Frame3D(-130, 80, 3),
                DefaultColor = Color.Green,
                IsMaterial = true,
                Density = Density.Iron,
                FrictionCoefficient = 8,
                Back = new SolidColorBrush { Color = Color.YellowGreen }
            };
            root.Add(first.Body);
            first.Body.Collision += body => first.AddScore(-1, "Столкновение");
            root.Add(new Box
            {
                XSize = 300,
                YSize = 200,
                ZSize = 3,
                DefaultColor = Color.White,
                Top = new SolidColorBrush{Color = Color.Yellow},
                IsStatic = true,
                Name = "floor",
            });
            root.Add(new Box
            {
                XSize = 10,
                YSize = 100,
                ZSize = 10,
                IsMaterial = true,
                DefaultColor = Color.DarkGray,
                Top = new SolidColorBrush { Color = Color.DarkGray },
                IsStatic = true,
                Name = "wall",
            });
            root.Add(new Box
            {
                XSize = 100,
                YSize = 10,
                ZSize = 10,
                IsMaterial = true,
                Location = new Frame3D(-100,0,0),
                DefaultColor = Color.DarkGray,
                Top = new SolidColorBrush { Color = Color.DarkGray },
                IsStatic = true,
                Name = "wall",
            });
            root.Add(new Box
            {
                XSize = 100,
                YSize = 10,
                ZSize = 10,
                Location = new Frame3D(100, 0, 0),
                DefaultColor = Color.DarkGray,
                Top = new SolidColorBrush { Color = Color.DarkGray },
                IsStatic = true,
                Name = "wall",
            });
            
            root.Add(new Box
            {
                XSize = 100,
                YSize = 10,
                ZSize = 10,
                Location = new Frame3D(0, 50, 0),
                IsStatic = true,
                IsMaterial = true,
                Name = "wall",
            });
            root.Add(new Box
            {
                Location = new Frame3D(100, 0, 0),
                Name = "place",
            });
            root.Add(new Box
            {
                Location = new Frame3D(-100, 0, 0),
                Name = "place",
            });
            root.GetSubtreeChildrenFirst().Where(a => a.Name == "place").OfType<Box>().ToList().ForEach(a =>
                {

                    a.XSize = 10;
                    a.YSize = 10;
                    a.ZSize = 15;
                    a.DefaultColor = Color.Cyan;
                    a.Top = new SolidColorBrush {Color = Color.Cyan};
                    a.IsStatic = true;
                    a.IsMaterial = true;
                });
            CreateBorders(root);
            CreateTreasure(root);
            return root;
        }
        
        private void CreateBorders(Body root)
        {
            Color wallsColor = Color.FromArgb(50, 0, 0, 0);
            for (int i = 0; i < 4; ++i)
            {
                var sizeX = i / 2 == 0 ? 303 : 3;
                var sizeY = i / 2 == 1 ? 203 : 3;
                var lX = i / 2 == 0 ? 203 : 3;
                var lY = i / 2 == 1 ? 303 : 3;
                var pos = i % 2 == 0 ? 1 : -1;
                root.Add(new Box
                {
                    XSize = sizeX,
                    YSize = sizeY,
                    ZSize = 3,
                    DefaultColor = wallsColor,
                    IsStatic = true,
                    Name = "wall",
                    IsMaterial = true,
                    Location = new Frame3D(
                        pos * lY / 2,
                        pos * lX / 2,
                        3)
                });
            }
        }
        private static void CreateTreasure(Body root)
        {
            root.Add(new Box
            {
                XSize = 8,
                YSize = 8,
                ZSize = 8,
                Location = new Frame3D(50, 80, 3),
                DefaultColor = Color.YellowGreen,
                IsMaterial = true,
                Name = "part",
                FrictionCoefficient = 8,
                Density = Density.Aluminum,
            });
            root.Add(new Box
            {
                XSize = 8,
                YSize = 8,
                ZSize = 8,
                Location = new Frame3D(-50, 80, 3),
                DefaultColor = Color.YellowGreen,
                IsMaterial = true,
                Name = "part",
                FrictionCoefficient = 8,
                Density = Density.Aluminum,
            });
            root.Add(new Box
            {
                XSize = 8,
                YSize = 8,
                ZSize = 8,
                Location = new Frame3D(50, -80, 3),
                DefaultColor = Color.Red,
                IsMaterial = true,
                Name = "part",
                FrictionCoefficient = 8,
                Density = Density.Aluminum,
            });
            root.Add(new Box
            {
                XSize = 8,
                YSize = 8,
                ZSize = 8,
                Location = new Frame3D(-50, -80, 3),
                DefaultColor = Color.Red,
                IsMaterial = true,
                Name = "part",
                FrictionCoefficient = 8,
                Density = Density.Aluminum
            });
            var radius = 60.0;
            var rand = new Random();

            for (int k = -1; k < 2; k += 2)
            {
                var gems = new List<Tuple<string, Color>>
                    {
                        new Tuple<string, Color>("part", Color.Red),
                        new Tuple<string, Color>("part", Color.YellowGreen),
                        null,
                        null,
                        null,
                        null,
                    };
                for (int i = 0; i < 6; i++)
                {
                    var ind = rand.Next(gems.Count);
                    var gem = gems[ind];
                    gems.RemoveAt(ind);
                    if (gem == null) continue;
                    var angleCircle = Angle.FromGrad(15 + 30 * i);
                    root.Add(new Box
                    {
                        XSize = 8,
                        YSize = 8,
                        ZSize = 8,
                        Location = new Frame3D(radius * k * Math.Sin(angleCircle.Radian), radius * Math.Cos(angleCircle.Radian), 3),
                        DefaultColor = gem.Item2,
                        IsMaterial = true,
                        Name = gem.Item1,
                        FrictionCoefficient = 8
                    });
                }
            }
        }
    }
}
