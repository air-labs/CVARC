using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Core;

namespace StarshipRepair
{
    public class GemsWorld : World
    {
        public static readonly Color WallColor=Color.LightGray;

        public override int RobotCount
        {
            get { return 2; }
        }

        public SceneSettings Settings { get; set; }


        void CreateWalls(Body root, WallSettings[,] array, int width, int height, int weight, string prefix, Func<int,int,Point> cooMaker)
        {
            for (int x = 0; x < array.GetLength(0); x++)
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    var wall = array[x, y];
                    if (wall == WallSettings.NoWall) continue;
                    Color color = Color.LightGray;
                    string name = prefix;
                    switch (wall)
                    {
                        case WallSettings.RedSocket: name += "R"; color = Color.DarkRed; break;
                        case WallSettings.BlueSocket: name += "B"; color = Color.DarkBlue; break;
                        case WallSettings.GreenSocket: name += "G"; color = Color.DarkGreen; break;
                    }
                    var coo = cooMaker(x, y);
                    root.Add(new Box
                    {
                        XSize = width,
                        YSize = height,
                        ZSize = weight,
                        Location = new Frame3D(coo.X, coo.Y, 0),
                        DefaultColor = color,
                        Name = name,
                        IsMaterial=true,
                        IsStatic=true,
                    });
                }

        }

        public override Body CreateWorld(IEnumerable<Robot> robots)
        {
            Settings = SceneSettings.GetDefaulSettings();
            var root = new Body();
            var enumerable = robots as IList<Robot> ?? robots.ToList();
            var first = enumerable[0];
            first.Body = new Cylinder
            {
                Height = 20,
                RTop = 10,
                RBottom = 10,
                Location = new Frame3D(-150+25-10, 100-25+10, 3),
                DefaultColor = Color.DarkViolet,
                IsMaterial = true,
                Density = Density.Iron,
                FrictionCoefficient = 0,
                Top = new SolidColorBrush { Color = Color.Red },
                Name="R1"
            };
            var second = enumerable[1];
            second.Body = new Cylinder
            {
                Height=20,
                RTop=10,
                RBottom=10,
                Location = new Frame3D(150 - 25 + 10, 100 - 25 + 10, 3, Angle.Zero, Angle.Pi, Angle.Zero),
                DefaultColor = Color.DarkViolet,
                IsMaterial = true,
                Density = Density.Iron,
                FrictionCoefficient = 0,
                Top = new SolidColorBrush { Color = Color.Blue },
                Name="R2"
            };
            root.Add(first.Body);
            root.Add(second.Body);
            first.Body.Collision += body => { if (body == second.Body) first.AddScore(-30, "Столкновение"); };
            second.Body.Collision += body => { if (body == first.Body) second.AddScore(-30, "Столкновение"); };
            
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

            foreach (var detail in Settings.Details)
            {
                Color color= Color.White;
                string name="D";
                switch(detail.Color)
                {
                    case DetailColor.Red: color=Color.Red; name+="R"; break;
                    case DetailColor.Blue: color=Color.Blue; name+="B"; break;
                    case DetailColor.Green: color=Color.Green; name+="G"; break;
                }

                root.Add(new Box
                {
                    XSize=15,
                    YSize=15,
                    ZSize=15,
                    Location=new Frame3D(-150+25+detail.Location.X*50,100-25-50*detail.Location.Y,0),
                    DefaultColor= color,
                     Name= name,
                     IsMaterial=true,
                     IsStatic=false
            });
            }

            CreateWalls(root, Settings.HorizontalWalls, 50, 10, 15, "HW", (x, y) => new Point(-150 + 25 + x * 50, 100 - (y + 1) * 50));
            CreateWalls(root, Settings.VerticalWalls, 10, 50, 14, "VW", (x, y) => new Point(-150 + (x+1) * 50, 100 - 25 - y * 50));


            CreateBorders(root);

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
