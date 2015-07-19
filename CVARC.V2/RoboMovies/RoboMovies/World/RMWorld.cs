using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;

namespace RoboMovies
{
    public class RMWorld : World<RMWorldState, IRMWorldManager>
     {
        public override void AdditionalInitialization()
        {
            var detector = new CollisionDetector(this);
            detector.FindControllableObject = side =>
                {
                    var robot = Actors
                        .OfType<ITowerBuilderRobot>()
                        .Where(z => z.ObjectId == side.ObjectId || z.TowerBuilder.CollectedIds.Contains(z.ObjectId))
                        .FirstOrDefault();
                    if (robot != null)
                    {
                        side.ControlledObjectId = robot.ObjectId;
                        side.ControllerId = robot.ControllerId;
                    }
                };
            detector.Account = c =>
                {
                    if (!c.Victim.IsControllable) return;
                    if (!detector.Guilty(c)) return;
                    Scores.Add(c.Offender.ControllerId, -30, "Collision");
                };
            
            Scores.Add(TwoPlayersId.Left, 0, "Staring scores");
            Scores.Add(TwoPlayersId.Right, 0, "Staring scores");
        }

        override public void CreateWorld()
        {
            Manager.CreateEmptyTable();
            
            foreach (var color in new[] { SideColor.Green, SideColor.Yellow })
            {
                var sideCorrection = GetSideCorrection(color);
                Manager.CreateStartingArea(new Point2D((150 - 20) * sideCorrection, 0), color);
                Manager.CreateStairs(IdGenerator.CreateNewId(new RMObject(color, ObjectType.Stairs)),
                    new Point2D(25 * sideCorrection, 100 - 32), color);
                CreateStands(sideCorrection, color);
                CreateLights(sideCorrection);
            }

            CreatePopCorn();
            CreateClapperboards();
        }

        public void CloseClapperboard(string clapperboardId)
        {
            Manager.CloseClapperboard(clapperboardId);
        }

        public bool IsInsideStartingArea(Frame3D location, SideColor color)
        {
            var loc2d = new Frame2D(Math.Abs(location.X), Math.Abs(location.Y), Angle.Zero);

            var insideSquare = loc2d.X >= 150 - 45 && loc2d.Y <= 20;
            var insideCircle = Math.Sqrt(Math.Pow(loc2d.X - 150 + 45, 2) + Math.Pow(loc2d.Y, 2)) <= 20;
            var correctSide = location.X * GetSideCorrection(color) > 0;

            return (insideSquare || insideCircle) && correctSide;
        }

        public bool IsInsideBuildingArea(Frame3D location)
        {
            return location.Y <= -100 + 20 && Math.Abs(location.X) <= 40;
        }

        private void CreateClapperboards()
        {
            for (var i = -3; i < 3; ++i)
            {
                var clapperOffset = i < 0 ? -30 : 60;
                var color = i % 2 == 0 ? SideColor.Green : SideColor.Yellow;

                Manager.CreateClapperboard(
                    IdGenerator.CreateNewId(new RMObject(color, ObjectType.Clapperboard)),
                    new Point2D(i * 30 + clapperOffset, -100 - 1),
                    color);
            }
        }

        private void CreateLights(int sideCorrection)
        {
            var light = new RMObject(SideColor.Any, ObjectType.Light);
            Manager.CreateLight(IdGenerator.CreateNewId(light), new Point2D((150 - 10) * sideCorrection, 0));
            Manager.CreateLight(IdGenerator.CreateNewId(light), new Point2D(20 * sideCorrection, 8 - 100));
        }

        private void CreatePopCorn()
        {
            var coords = new List<Point2D>()
            {
                new Point2D(50, 15),
                new Point2D(110, -80),
                new Point2D(0, -70),
            };

            var allCoords = coords
                .Union(coords.Select(p => new Point2D(p.X * -1, p.Y)))
                .Distinct();
            
            foreach (var point in allCoords)
                Manager.CreatePopCorn(IdGenerator.CreateNewId(new RMObject(SideColor.Any, ObjectType.PopCorn)), point);

        }

        private void CreateStands(int sideCorrection, SideColor color)
        {
            var coords = new List<Point2D>()
            {
                new Point2D(20, -40),
                new Point2D(50, -40),
                new Point2D(35, -60),
                new Point2D(50, 75),
                new Point2D(50, 90),
                new Point2D(140, -75),
                new Point2D(140, -90),
                new Point2D(140, 75),
            };

            foreach (var coord in coords)
                Manager.CreateStand(IdGenerator.CreateNewId(new RMObject(color, ObjectType.Stand)),
                    new Point2D(coord.X * sideCorrection, coord.Y), color);
        }

        private int GetSideCorrection(SideColor color)
        {
            return color == SideColor.Yellow ? -1 : 1;
        }
    }
}