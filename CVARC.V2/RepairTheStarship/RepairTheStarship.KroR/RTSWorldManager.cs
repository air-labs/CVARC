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
          
        }

        public static Color DefaultWallColor { get { return Color.LightGray; } }



   



        public void CreateEmptyTable()
        {

            Root.Add(new Box
            {
                XSize = 300,
                YSize = 200,
                ZSize = 3,
                DefaultColor = Color.White,
                Top = new SolidColorBrush { Color = Color.Yellow },
                IsStatic = true,
                NewId = "floor"
            });

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

        public void CreateWall(string wallId, Point2D centerLocation, WallData settings)
        {
                    Color color = DefaultWallColor;
                    switch (settings.Type)
                    {
                        case WallSettings.RedSocket: color = Color.DarkRed; break;
                        case WallSettings.BlueSocket: color = Color.DarkBlue; break;
                        case WallSettings.GreenSocket: color = Color.DarkGreen; break;
                    }

            int width = 50;
            int height = 10;
            int weigth = 15;

            if (settings.Orientation == WallOrientation.Vertical)
            {
                width=10;
                height=50;
                weigth=14;
            }

                    Root.Add(new Box
                    {
                        XSize = width,
                        YSize = height,
                        ZSize = 15,
                        Location = new Frame3D(centerLocation.X,centerLocation.Y,0),
                        DefaultColor = color,
                        NewId = wallId,
                        IsMaterial = true,
                        IsStatic = true,
                    });
        }

        public void CreateDetail(string detailId, Point2D detailLocation, DetailColor dcolor)
        {
            Color color = Color.White;

            switch (dcolor)
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
                Location = new Frame3D(detailLocation.X,detailLocation.Y,0),
                DefaultColor = color,
                NewId = detailId,
                IsMaterial = true,
                IsStatic = false,
                FrictionCoefficient = 8
            };
            Root.Add(box);
        }

     
    }
}
