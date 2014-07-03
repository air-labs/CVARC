using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Core;

namespace StarshipRepair
{
    public class GemsEngine : CVARCEngine
    {
        SceneSettings Settings;

        #region World creation
        void CreateWalls(Body root, WallSettings[,] array, int width, int height, int weight, string prefix, Func<int, int, Point> cooMaker)
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
                        IsMaterial = true,
                        IsStatic = true,
                    });
                }
        }

        private Stream GetResourceStream(string resourceName)
        {
            return GetType().Assembly.GetManifestResourceStream("Gems.Resources." + resourceName);
        }


        public override Body CreateWorld(ISceneSettings _settings)
        {
            Settings = (SceneSettings)_settings;
            var root = new Body();

            var first = new Cylinder
            {
                Height = 20,
                RTop = 10,
                RBottom = 10,
                Location = new Frame3D(-150 + 25 - 10, 100 - 25 + 10, 3),
                DefaultColor = Color.DarkViolet,
                IsMaterial = true,
                Density = Density.Iron,
                FrictionCoefficient = 0,
                Top = new PlaneImageBrush { Image = new Bitmap(GetResourceStream("red.png")) },
                Name = "Robot0"
            };
            var second = new Cylinder
            {
                Height = 20,
                RTop = 10,
                RBottom = 10,
                Location = new Frame3D(150 - 25 + 10, 100 - 25 + 10, 3, Angle.Zero, Angle.Pi, Angle.Zero),
                DefaultColor = Color.DarkViolet,
                IsMaterial = true,
                Density = Density.Iron,
                FrictionCoefficient = 0,
                Top = new PlaneImageBrush { Image = new Bitmap(GetResourceStream("blue.png")) },
                Name = "Robot1"
            };
            root.Add(first);
            root.Add(second);

            //Это какой-то пиздец. С коллизиями позже разберемся
            //var dt = 1000;
            //var collisionTimes = new Dictionary<int, DateTime>
            //                                                {
            //                                                    {first.Body.Id,new DateTime()},
            //                                                    {second.Body.Id,new DateTime()}
            //                                                };
            //Func<Body, Body, bool> isFault = (robot, opponent) =>
            //                                     {
            //                                         var vec = opponent.GetAbsoluteLocation() - robot.GetAbsoluteLocation();
            //                                         var sc = vec.X*robot.Velocity.X + vec.Y*robot.Velocity.Y;
            //                                         return sc > 0;
            //                                     };
            //Func<Robot, Robot, Action<Body>> subscribeToCollision = (robot, opponent) =>
            //    body =>
            //        {
            //            if (body == opponent.Body && isFault(robot.Body, body) &&
            //                (DateTime.Now - collisionTimes[robot.Body.Id]).TotalMilliseconds > dt)
            //            {
            //                robot.AddScore(-30, "Collision");
            //                collisionTimes[robot.Body.Id] = DateTime.Now;
            //            }
            //        };
            //first.Body.Collision += subscribeToCollision(first, second);
            //second.Body.Collision += subscribeToCollision(second, first);
            //root.Add(new Box
            //{
            //    XSize = 300,
            //    YSize = 200,
            //    ZSize = 3,
            //    DefaultColor = Color.White,
            //    Top = new SolidColorBrush{Color = Color.Yellow},
            //    IsStatic = true,
            //    Name = "floor",
            //});

            foreach (var detail in Settings.Details)
            {
                Color color = Color.White;
                string name = "D";
                switch (detail.Color)
                {
                    case DetailColor.Red: color = Color.Red; name += "R"; break;
                    case DetailColor.Blue: color = Color.Blue; name += "B"; break;
                    case DetailColor.Green: color = Color.Green; name += "G"; break;
                }

                root.Add(new Box
                {
                    XSize = 15,
                    YSize = 15,
                    ZSize = 15,
                    Location = new Frame3D(-150 + 25 + detail.Location.X * 50, 100 - 25 - 50 * detail.Location.Y, 0),
                    DefaultColor = color,
                    Name = name,
                    IsMaterial = true,
                    IsStatic = false,
                    FrictionCoefficient = 8
                });
            }

            CreateWalls(root, Settings.HorizontalWalls, 50, 10, 15, "HW", (x, y) => new Point(-150 + 25 + x * 50, 100 - (y + 1) * 50));
            CreateWalls(root, Settings.VerticalWalls, 10, 50, 14, "VW", (x, y) => new Point(-150 + (x + 1) * 50, 100 - 25 - y * 50));


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

        #endregion

        #region World manipulation

        public override void PerformAction(string actor, string action)
        {
            var robotBody = GetBody(actor);
            if (action == "Grip") Grip(robotBody);
            if (action == "Release") Release(robotBody);
        }


        private void Release(Body Body)
        {
            var latestGripped = Body.FirstOrDefault(z => z.Name.StartsWith("D") && z.Name.Length == 2);
            if (latestGripped == null) return;

            var absoluteLocation = latestGripped.GetAbsoluteLocation();
            Body.Remove(latestGripped);

            var targetColor = latestGripped.Name[1].ToString();

            latestGripped.Location = absoluteLocation;
            latestGripped.Velocity = Body.Velocity;
            var toAtt = Body.TreeRoot.GetSubtreeChildrenFirst()
                            .Where(a =>
                                    (a.Name == "VW" + targetColor || a.Name == "HW" + targetColor) &&
                                    Distance(latestGripped, a) < 30)
                            .OfType<Box>()
                            .FirstOrDefault();

            if (toAtt != null)
            {
                Body.TreeRoot.Remove(toAtt);
                var wall = new Box
                {
                    Name = toAtt.Name.Substring(0, 2),
                    XSize = toAtt.XSize,
                    YSize = toAtt.YSize,
                    ZSize = toAtt.ZSize,
                    Location = toAtt.Location,
                    DefaultColor = GemsWorld.WallColor,
                    IsStatic = true,
                    IsMaterial = true
                };
                Body.TreeRoot.Add(wall);

                //С очками позже разберемся
                //AddScore(10, "Repaired wall " + targetColor);
            }
            else
                Body.TreeRoot.Add(latestGripped);
            //  gripped.RemoveRange(0, gripped.Count);
        }

        private void Grip(Body Body)
        {
            var gripped = Body.ToList();
            if (gripped.Any()) return;
            var found = Body.TreeRoot.GetSubtreeChildrenFirst().FirstOrDefault(a => CanBeAttached(Body, a) && (a.Parent == a.TreeRoot || !a.Parent.IsStatic));
            if (found != null)
            {
                Body latestGripped = null;
                var oldGrippedCount = gripped.Count;
                if (oldGrippedCount > 0)
                {
                    latestGripped = gripped.Last();
                    if (found.DefaultColor != latestGripped.DefaultColor)
                        return;
                }

                var tempfound = found;

                while (tempfound.Any())
                {
                    tempfound = tempfound.FirstOrDefault();
                    gripped.Add(tempfound);
                }

                if (oldGrippedCount > 0)
                {
                    Body.Remove(latestGripped);
                    if (latestGripped != null)
                    {
                        latestGripped.Location = new Frame3D(0, 0, 8, Angle.Zero, Angle.Zero, Angle.Zero);
                        tempfound.Add(latestGripped);
                    }
                }

                CaptureDevicet(Body, found);
                gripped.Add(found);
            }

        }

        private bool CanBeAttached(Body to, Body body)
        {
            return body != to &&
                !body.IsStatic &&
                !to.SubtreeContainsChild(body) &&
                !to.ParentsContain(body) &&
                body.Name.StartsWith("D") &&
                Distance(body, to) < 30;
        }

        private double Distance(Body from, Body to)
        {
            return Angem.Hypot(from.GetAbsoluteLocation() - to.GetAbsoluteLocation());
        }


        private void CaptureDevicet(Body box, Body newChild)
        {
            var childAbsolute = newChild.GetAbsoluteLocation();
            if (newChild.Parent != null)
                newChild.Parent.Remove(newChild);
            newChild.Location = box.GetAbsoluteLocation().Invert().Apply(childAbsolute);
            newChild.Location = newChild.Location.NewYaw(Angle.Zero);
            newChild.Location = newChild.Location.NewX(14);
            newChild.Location = newChild.Location.NewY(0);
            box.Add(newChild);
        }

        #endregion
    }
}
