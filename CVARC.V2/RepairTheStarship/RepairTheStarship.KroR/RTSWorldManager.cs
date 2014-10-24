using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRLab.Mathematics;
using CVARC.Core;
using CVARC.V2;

namespace RepairTheStarship.KroR
{


    public class RTSWorldManager : KroRWorldManager<RTSWorld>, IRTSWorldManager
    {
        SceneSettings Settings;
        IdGenerator generator;

        public void RemoveDetail(string detailId)
        {
            var engine = (Engine as KroREngine);
            var detailBody = engine.GetBody(detailId);
            engine.Root.Remove(detailBody);
        }

        public void ShutTheWall(string wallId)
        {
            var engine = (Engine as KroREngine);
            var wall = engine.GetBody(wallId) as Box;
            var wallData = World.IdGenerator.GetKey<WallData>(wallId);
            var newWall = new Box
            {
                XSize = wall.XSize,
                YSize = wall.YSize,
                ZSize = wall.ZSize,
                Location = wall.Location,
                DefaultColor = RTSWorldManager.DefaultWallColor,
                IsStatic = true,
                IsMaterial = true,
                NewId = World.IdGenerator.CreateNewId(new WallData { Orientation = wallData.Orientation, Type = WallSettings.Wall })
            };
            engine.Root.Remove(wall);
            engine.Root.Add(newWall);
        }

        public override void CreateWorld(IdGenerator generator)
        {
            this.generator = generator;
            Settings = World.SceneSettings;
            

            
            Root.Add(new Box
            {
                XSize = 300,
                YSize = 200,
                ZSize = 3,
                DefaultColor = Color.White,
                Top = new SolidColorBrush { Color = Color.Yellow },
                IsStatic = true,
                NewId="floor"
            });

            foreach (var detail in Settings.Details)
            {
                Color color = Color.White;
               
                switch (detail.Color)
                {
                    case DetailColor.Red: color = Color.Red; break;
                    case DetailColor.Blue: color = Color.Blue; break;
                    case DetailColor.Green: color = Color.Green; break;
                }

                var box = new Box
                {
                    XSize = 15,
                    YSize = 15,
                    ZSize = 15,
                    Location = new Frame3D(-150 + 25 + detail.Location.X * 50, 100 - 25 - 50 * detail.Location.Y, 0),
                    DefaultColor = color,
                    NewId = generator.CreateNewId(detail.Color),
                    IsMaterial = true,
                    IsStatic = false,
                    FrictionCoefficient = 8
                };
                Root.Add(box);
            }

            CreateWalls(Settings.HorizontalWalls, 50, 10, 15, WallOrientation.Horizontal, (x, y) => new Point(-150 + 25 + x * 50, 100 - (y + 1) * 50));
            CreateWalls(Settings.VerticalWalls, 10, 50, 14, WallOrientation.Vertical, (x, y) => new Point(-150 + (x + 1) * 50, 100 - 25 - y * 50));
            CreateBorders();
        }

        public static Color DefaultWallColor { get { return Color.LightGray; } }

        void CreateWalls(WallSettings[,] array, int width, int height, int weight, WallOrientation orientation, Func<int, int, Point> cooMaker)
        {
            for (int x = 0; x < array.GetLength(0); x++)
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    var wall = array[x, y];
                    if (wall == WallSettings.NoWall) continue;

                    Color color = DefaultWallColor;
                    switch (wall)
                    {
                        case WallSettings.RedSocket: color = Color.DarkRed; break;
                        case WallSettings.BlueSocket: color = Color.DarkBlue; break;
                        case WallSettings.GreenSocket: color = Color.DarkGreen; break;
                    }

                    var data = new WallData { Orientation = orientation, Type = wall };
                    var coo = cooMaker(x, y);
                    Root.Add(new Box
                    {
                        XSize = width,
                        YSize = height,
                        ZSize = weight,
                        Location = new Frame3D(coo.X, coo.Y, 0),
                        DefaultColor = color,
                        NewId = generator.CreateNewId(data),
                        IsMaterial = true,
                        IsStatic = true,
                    });
                }
        }

   

        private void CreateBorders()
        {
            Color wallsColor = Color.FromArgb(50, 0, 0, 0);
            for (int i = 0; i < 4; ++i)
            {
                var sizeX = i / 2 == 0 ? 303 : 3;
                var sizeY = i / 2 == 1 ? 203 : 3;
                var lX = i / 2 == 0 ? 203 : 3;
                var lY = i / 2 == 1 ? 303 : 3;
                var pos = i % 2 == 0 ? 1 : -1;
                Root.Add(new Box
                {
                    XSize = sizeX,
                    YSize = sizeY,
                    ZSize = 3,
                    DefaultColor = wallsColor,
                    IsStatic = true,
                    IsMaterial = true,
                    Location = new Frame3D(
                        pos * lY / 2,
                        pos * lX / 2,
                        3)
                });
            }
        }



    }
}
