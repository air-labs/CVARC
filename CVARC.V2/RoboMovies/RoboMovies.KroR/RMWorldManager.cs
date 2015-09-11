using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using AIRLab.Mathematics;
using CVARC.Core;
using CVARC.V2;


namespace RoboMovies.KroR
{
    public static class WorldInitializerHelper
    {
        public static Stream GetResourceStream(string resourceName)
        {
            var assembly = typeof(WorldInitializerHelper).Assembly;
            var names = assembly.GetManifestResourceNames();
            return assembly.GetManifestResourceStream("RoboMovies.KroR.Resources." + resourceName);
        }
    }

    public class RMWorldManager : KroRWorldManager<RMWorld>, IRMWorldManager
    {
        static int length = 300;
        static int width = 200;
        static int halfLength = length / 2;
        static int halfWidth = width / 2;
        static int floorLevel = 3;

        public void RemoveObject(string objectId)
        {
            var engine = (Engine as KroREngine);
            var objectBody = engine.GetBody(objectId);
            engine.Root.Remove(objectBody);
        }

        public override void CreateWorld(IdGenerator generator)
        {
          
        }

        public void CreateEmptyTable()
        {
            var floorImage = new Bitmap(WorldInitializerHelper.GetResourceStream("field.png"));
            floorImage.RotateFlip(RotateFlipType.Rotate90FlipNone);

            Root.Add(new Box
            {
                XSize = length,
                YSize = width,
                ZSize = floorLevel,
                DefaultColor = Color.White,
                Density = Density.Wood,
                Top = new PlaneImageBrush { Image = floorImage },
                IsStatic = true,
                NewId = "floor",
            });

            CreateBorders();
        }

        private void CreateBorders()
        {
            Color wallsColor = Color.DarkRed;
            for (int i = 0; i < 4; ++i)
            {
                var sizeX = i / 2 == 0 ? length + 2.2 : 2.2;
                var sizeY = i / 2 == 1 ? width + 2.2 : 2.2;
                var lX = i / 2 == 0 ? width + 2.2 : 2.2;
                var lY = i / 2 == 1 ? length + 2.2 : 2.2;
                var pos = i % 2 == 0 ? 1 : -1;
                Root.Add(new Box
                {
                    XSize = sizeX,
                    YSize = sizeY,
                    ZSize = 10,
                    DefaultColor = wallsColor,
                    IsStatic = true,
                    IsMaterial = true,
                    Density = Density.Iron,
                    Location = new Frame3D(
                        pos * lY / 2,
                        pos * lX / 2,
                        floorLevel)
                });
            }
        }

        private Color GetDrawingColor(SideColor color)
        {
            if (color == SideColor.Green) return Color.Green;
            else return Color.Yellow;
        }

        public void CreateStartingArea(Point2D centerLocation, SideColor color)
        {
            var drawingColor = GetDrawingColor(color);
            var offset = new Frame3D(centerLocation.X, centerLocation.Y, floorLevel);
            
            var sideCorrection = color == SideColor.Green ? 1 : -1;
            var XSize = 40;
            var YSize = 40;
            
            var entities = new List<Box>(); 
            
            entities.Add(new Box
            {
                XSize = XSize,
                YSize = 2.2,
                ZSize = 2.2,
                Location = new Frame3D(0, YSize / 2, 0),
            });
            
            entities.Add(new Box
            {
                XSize = XSize,
                YSize = 2.2,
                ZSize = 2.2,
                Location = new Frame3D(0, -YSize / 2, 0),
            });

            foreach (var wall in entities)
            {
                wall.IsStatic = true;
                wall.IsMaterial = true;
                wall.NewId = "Wall";
                wall.DefaultColor = drawingColor;
                wall.Location += offset;
                Root.Add(wall);
            }
        }

        public void CreateStairs(string stairsId, Point2D centerLocation, SideColor color)
        {
            var drawingColor = GetDrawingColor(color);
            var offset = new Frame3D(centerLocation.X, centerLocation.Y, floorLevel);
        
            double ZSize = 7;
            Func<double, double> getZSize = y => -ZSize / (7 * 4) * (y - 39) + ZSize;

            double width = 50;
         	double bottomLength = 60;
         	double topLength = bottomLength - 21;
         	int stairsCount = 3;
            var stairStep = (bottomLength - topLength) / stairsCount;
            
            for (var ySize = topLength; ySize <= bottomLength; ySize += stairStep)
                Root.Add(new Box
                {
                    XSize = width,
                    YSize = ySize,
                    ZSize = getZSize(ySize),
                    Location = new Frame3D(0, (bottomLength - ySize) / 2, 0) + offset,
                    DefaultColor = drawingColor,
                    IsStatic = true,
                    IsMaterial = true,
                    NewId = ySize == topLength ? stairsId : "wall",
                });

            Func<double, Box> getBorder = x => new Box
            {
                XSize = 3,
                YSize = bottomLength,
                ZSize = ZSize + 2.2,
                Location = new Frame3D(x, 0, 0) + offset,
                IsMaterial = true,
                IsStatic = true,
                DefaultColor = Color.CornflowerBlue,
                NewId = "wall",
            };

            Root.Add(getBorder(width / 2));
            Root.Add(getBorder(-width / 2));
        }

        public void CreateLight(string lightId, Point2D location)
        {
            Root.Add(new Ball
            {
                Radius = 3.2,
                Location = new Frame3D(location.X, location.Y, floorLevel * 2),
                DefaultColor = Color.GreenYellow,
                NewId = lightId,
                IsMaterial = true,
                Density = Density.PlasticPvc,
                FrictionCoefficient = 10,
            });
        }

        public void CreateStand(string standId, Point2D location, SideColor color)
        {
            Root.Add(new Cylinder
            {
                RTop = 3,
                RBottom = 3,
                Height = 7,
                IsStatic = false,
                IsMaterial = true,
                Density = Density.Iron,
                Location = new Frame3D(location.X, location.Y, floorLevel),
                DefaultColor = GetDrawingColor(color),
                FrictionCoefficient = 10,
                NewId = standId,
            });
        }
        
        public void CreatePopCorn(string popcornId, Point2D location)
        {
            Root.Add(new Cylinder
            {
                RTop = 9.5 / 2,
                RBottom = 5.4 / 2,
                Height = 14,
                IsStatic = false,
                IsMaterial = true,
                Density = Density.PlasticPvc,
                Location = new Frame3D(location.X, location.Y, floorLevel),
                DefaultColor = Color.White,
                FrictionCoefficient = 10,
                NewId = popcornId,
            });
        }

        public void CreateClapperboard(string clapperboardId, Point2D location, SideColor color)
        {
            var drawingColor = GetDrawingColor(color);
            var mainBodyLocation = new Frame3D(location.X, location.Y, floorLevel);
            var clapperboard = new Clapperboard(mainBodyLocation, drawingColor, clapperboardId);
            Root.Add(clapperboard);

            var clapperBoard = new Box
            {
                XSize = 16,
                YSize = 5,
                ZSize = 10,
                DefaultColor = Color.Black,
                Location = mainBodyLocation,
                IsStatic = true,
                NewId = clapperboardId,
            };

            var cap = new Box
                {
                    XSize = 3,
                    YSize = 5,
                    ZSize = 16,
                    DefaultColor = drawingColor,
                    Location = new Frame3D(8, 0, 16, Angle.Pi / 6, Angle.Zero, Angle.Zero) + mainBodyLocation,
                    IsStatic = true,
                    NewId = "cap",
                };

            clapperboard.Add(cap);
            Root.Add(clapperboard);
        }

        public void CloseClapperboard(string clapperboardId)
        {
            var engine = Engine as KroREngine;
            var clapperboard = engine.GetBody(clapperboardId) as Box;
            if (clapperboardId != null)
            {
                var cap = clapperboard.Nested.First();
                clapperboard.Remove(cap);
                clapperboard.Add(new Box
                {
                    XSize = 16,
                    YSize = 5,
                    ZSize = 3,
                    DefaultColor = cap.DefaultColor,
                    Location = new Frame3D(0, 0, clapperboard.ZSize),
                    IsStatic = true,
                    NewId = "CC",
                });
            }
        }

        public void CreatePopCornDispenser(string id, Point2D location)
        {
            Root.Add(new Box
            {
                XSize = 6,
                YSize = 6,
                ZSize = 28,
                IsStatic = true,
                IsMaterial = true,
                Location = new Frame3D(location.X, location.Y, floorLevel),
                DefaultColor = Color.CornflowerBlue,
                NewId = id,
            });
        }

        public void ClimbUpStairs(string actorId, string stairsId)
        {
            Engine.Attach(actorId, stairsId, new Frame3D(0, 0, 7));
        }
    }
}
