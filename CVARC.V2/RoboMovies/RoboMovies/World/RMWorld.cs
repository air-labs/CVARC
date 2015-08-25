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
        public Dictionary<string, int> PopCornFullness = new Dictionary<string, int>();
        public Dictionary<string, HashSet<string>> Spotlights = new Dictionary<string, HashSet<string>>();
        public HashSet<string> ClosedClapperboards = new HashSet<string>();
        
        public int CupCapacity { get { return 10; } }
        const int defaultCupFullness = 4;
        const int defaultDispenserFullness = 5;
        
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

            Clocks.AddTrigger(new RMScoresTrigger(this));
        }

        // TODO: create RMWorldHelper.cs
        #region WorldHelper
        public void CloseClapperboard(string clapperboardId)
        {
            Manager.CloseClapperboard(clapperboardId);
        }

        public bool IsValidStand(Frame3D location, string controllerId)
        {
            var color = GetColorByController(controllerId);
            return IsInsideStartingArea(location, color) || IsInsideBuildingArea(location);
        }
        
        public bool IsValidPopCorn(Frame3D location, string controllerId)
        {
            var color = GetColorByController(controllerId);
            return IsInsideCinema(location, color) || IsInsideStartingArea(location, color);
        }
        
        public bool IsValidPopCorn(string popcornId, string controllerId, out Cinema cinema)
        {
            var location = Engine.GetAbsoluteLocation(popcornId);
            cinema = GetCinemaByLocation(location);
            return IsValidPopCorn(location, controllerId);
        }

        public bool IsInsideStartingArea(Frame3D location, SideColor color)
        {
            var loc2d = GetSideIndependentLocation(location);

            var insideSquare = loc2d.X >= 150 - 45 && loc2d.Y <= 20;
            var insideCircle = Math.Sqrt(Math.Pow(loc2d.X - 150 + 45, 2) + Math.Pow(loc2d.Y, 2)) <= 20;
            var correctSide = location.X * GetSideCorrection(color) > 0;

            return (insideSquare || insideCircle) && correctSide;
        }

        public bool IsInsideBuildingArea(Frame3D location)
        {
            return location.Y <= -100 + 20 && Math.Abs(location.X) <= 40;
        }

        bool IsInsideCinema(Frame3D location, SideColor color)
        {
            var loc2d = GetSideIndependentLocation(location);

            var insideSquare = loc2d.X >= 150 - 45 && loc2d.Y >= 20 && loc2d.Y <= 60;
            var correctSide = location.X * GetSideCorrection(color) < 0;

            return correctSide && insideSquare;
        }

        Cinema GetCinemaByLocation(Frame3D location)
        {
            if (IsInsideCinema(location, SideColor.Yellow))
                if (location.Y > 0)
                    return Cinema.UpperRight;
                else
                    return Cinema.BottomRight;

            if (IsInsideCinema(location, SideColor.Green))
                if (location.Y > 0)
                    return Cinema.UpperLeft;
                else
                    return Cinema.BottomLeft;

            if (IsInsideStartingArea(location, SideColor.Green))
                return Cinema.RightStartingArea;
            if (IsInsideStartingArea(location, SideColor.Yellow))
                return Cinema.LeftStartingArea;
            
            return Cinema.None;
        }

        Frame2D GetSideIndependentLocation(Frame3D location)
        {
            return new Frame2D(Math.Abs(location.X), Math.Abs(location.Y), Angle.Zero);
        }

        SideColor GetColorByController(string twoPlayersId)
        {
            if (twoPlayersId == TwoPlayersId.Left)
                return SideColor.Yellow;
            if (twoPlayersId == TwoPlayersId.Right)
                return SideColor.Green;
            throw new ArgumentException();
        }

        public RMObject GetObjectById(string id)
        {
            if (!IdGenerator.KeyOfType<RMObject>(id))
                throw new ArgumentException("This id is not binded to any RMObject.");
            return IdGenerator.GetKey<RMObject>(id);
        }
        #endregion

        #region WorldCreation
        override public void CreateWorld()
        {
            Manager.CreateEmptyTable();
            
            foreach (var color in new[] { SideColor.Green, SideColor.Yellow })
            {
                var sideCorrection = GetSideCorrection(color);
                Manager.CreateStartingArea(new Point2D((150 - 20) * sideCorrection, 0), color);
                Manager.CreateStairs(IdGenerator.CreateNewId(new RMObject(color, ObjectType.Stairs)),
                    new Point2D(25 * sideCorrection, 100 - 30), color);
                CreateStands(sideCorrection, color);
                CreateLights(sideCorrection);
                CreatePopCornDispensers(sideCorrection);
            }

            CreatePopCorn();
            CreateClapperboards();
        }

        private void CreateClapperboards()
        {
            for (var i = -3; i < 3; ++i)
            {
                var clapperOffset = i < 0 ? -30 : 60;
                var color = i % 2 == 0 ? SideColor.Green : SideColor.Yellow;
                var clapperboardId = IdGenerator.CreateNewId(new RMObject(color, ObjectType.Clapperboard));

                Manager.CreateClapperboard(clapperboardId,
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
                new Point2D(50, 20),
                new Point2D(110, -80),
                new Point2D(0, -70),
            };

            var allCoords = coords
                .Union(coords.Select(p => new Point2D(p.X * -1, p.Y)))
                .Distinct();

            foreach (var point in allCoords)
            {
                var id = IdGenerator.CreateNewId(new RMObject(SideColor.Any, ObjectType.PopCorn));
                Manager.CreatePopCorn(id, point);
                PopCornFullness[id] = defaultCupFullness;
            }
        }

        private void CreatePopCornDispensers(int sideCorrection)
        {
            var step = 33;
            for (var x = 1; x < 3; ++x)
            {
                var id = IdGenerator.CreateNewId(new RMObject(SideColor.Any, ObjectType.Dispenser));
                PopCornFullness[id] = defaultDispenserFullness;
                Manager.CreatePopCornDispenser(id, new Point2D((150 - x * step) * sideCorrection, 100));
            }
        }

        private void CreateStands(int sideCorrection, SideColor color)
        {
            var coords = new List<Point2D>()
            {
                new Point2D(20, -40),
                new Point2D(50, -40),
                new Point2D(35, -60),
                new Point2D(60, 75),
                new Point2D(60, 90),
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
        #endregion
     }
}
