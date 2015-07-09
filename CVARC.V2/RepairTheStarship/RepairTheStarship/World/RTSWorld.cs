using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2;

namespace RepairTheStarship
{
    public class RTSWorld : World<RTSWorldState, IRTSWorldManager>
     {
        Dictionary<DetailColor, int> repairs = new Dictionary<DetailColor, int>();


        public RTSWorld()
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
            Manager.RemoveDetail(detailId);
            Manager.ShutTheWall(wallId);
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

            var SceneSettings = RepairTheStarship.SceneSettings.GetRandomMap(WorldState.Seed);
            Manager.CreateEmptyTable();
            foreach (var e in SceneSettings.Details)
                Manager.CreateDetail(
                    IdGenerator.CreateNewId(e.Color),
                    new Point2D(-150 + e.Location.X * 50+25, 100 - 25 - 50 * e.Location.Y),
                    e.Color);
            for (int x = 0; x < SceneSettings.HorizontalWalls.GetLength(0); x++)
                for (int y = 0; y < SceneSettings.HorizontalWalls.GetLength(1); y++)
                    if (SceneSettings.HorizontalWalls[x, y] != WallSettings.NoWall)
                    {
                        var settings = new WallData { Orientation = WallOrientation.Horizontal, Type = SceneSettings.HorizontalWalls[x, y] };
                        var id = IdGenerator.CreateNewId(settings);
                        Manager.CreateWall(id, new Point2D(-150 + 25 + x * 50, 100 - (y + 1) * 50), settings);
                    }
            for (int x = 0; x < SceneSettings.HorizontalWalls.GetLength(0); x++)
                for (int y = 0; y < SceneSettings.HorizontalWalls.GetLength(1); y++)
                    if (SceneSettings.HorizontalWalls[x, y] != WallSettings.NoWall)
                    {
                        var settings = new WallData { Orientation = WallOrientation.Horizontal, Type = SceneSettings.HorizontalWalls[x, y] };
                        var id = IdGenerator.CreateNewId(settings);
                        Manager.CreateWall(id, new Point2D(-150 + 25 + x * 50, 100 - (y + 1) * 50), settings);
                    }
            for (int x = 0; x < SceneSettings.VerticalWalls.GetLength(0); x++)
                for (int y = 0; y < SceneSettings.VerticalWalls.GetLength(1); y++)
                    if (SceneSettings.VerticalWalls[x, y] != WallSettings.NoWall)
                    {
                        var settings = new WallData { Orientation = WallOrientation.Vertical, Type = SceneSettings.VerticalWalls[x, y] };
                        var id = IdGenerator.CreateNewId(settings);
                        Manager.CreateWall(id, new Point2D(-150 + (x + 1) * 50, 100 - 25 - y * 50), settings);
                    }
        }

        
    }

}