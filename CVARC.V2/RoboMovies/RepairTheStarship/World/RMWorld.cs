using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;

namespace RoboMovies
{
    public class RMWorld : World<RTSWorldState, IRTSWorldManager>
     {
        Dictionary<DetailColor, int> repairs = new Dictionary<DetailColor, int>();

        public RMWorld()
        {
            repairs[DetailColor.Blue] = 0;
            repairs[DetailColor.Green] = 0;
            repairs[DetailColor.Red] = 0;
        }

        public override void AdditionalInitialization()
        {
            var detector = new CollisionDetector(this);
            detector.FindControllableObject = side =>
                {
                    var robot = Actors
                        .OfType<IGrippableRobot>()
                        .Where(z => z.ObjectId == side.ObjectId || z.Gripper.GrippedObjectId == side.ObjectId)
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

        public void InstallDetail(DetailColor color, string detailId, string wallId, string robotId)
        {
            //Manager.RemoveDetail(detailId);
            //Manager.ShutTheWall(wallId);
            Scores.Add(robotId, 10, "Repaired " + color);
            repairs[color]++;
            if (repairs[color] == 2)
            {
                Scores.Add(TwoPlayersId.Left, 5, "Cooperative action for color " + color);
                Scores.Add(TwoPlayersId.Right, 5, "Cooperative action for color " + color);
            }
            if (repairs.Values.Sum() == 6)
            {
                Scores.Add(TwoPlayersId.Left, 5, "Cooperative action for all colors");
                Scores.Add(TwoPlayersId.Right, 5, "Cooperative action for all colors");
            }
        }

        override public void CreateWorld()
        {
            Manager.CreateEmptyTable();
            
            foreach (var color in new[] { SideColor.Green, SideColor.Yellow })
            {
                var sideCorrection = GetSideCorrection(color);
                Manager.CreateStartingArea(new Point2D((150 - 20) * sideCorrection, 0), color);
                Manager.CreateStairs(IdGenerator.CreateNewId(new RMObject(color, ObjectType.Stairs)),
                    new Point2D(20 * sideCorrection, 100 - 32), color);
                CreateStands(sideCorrection, color);
                CreateLights(sideCorrection);
            }

            CreateClapperboards();
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
            Manager.CreateLight(IdGenerator.CreateNewId(light), new Point2D((150 - 5) * sideCorrection, 0));
            Manager.CreateLight(IdGenerator.CreateNewId(light), new Point2D(20 * sideCorrection, 8 - 100));
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