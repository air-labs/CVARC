using System;
using System.Collections.Generic;
using System.Linq;
using CVARC.V2;
using RepairTheStarship.MapBuilder;

namespace RepairTheStarship.Bots
{
    public class Azura : RepairTheStarshipBot
    {
        private string color;
        private bool hasDetail;
        private const int Epsilon = 30;

        protected override IEnumerable<MoveAndGripCommand> FindNextCommands()
        {
            return hasDetail ? RepairStarship() : GripDetail(); 
        }

        private IEnumerable<MoveAndGripCommand> RepairStarship()
        {
            var nearSocket = Map.Walls.Where(x => x.Type.ToLower().Contains(color)).OrderBy(GetDistance).FirstOrDefault();
            if (nearSocket == null)
                return new MoveAndGripCommand[0];
            var path = PathSearcher.FindPath(Map, OurCoordinates, nearSocket.DiscreteCoordinate);
            if (path.Length == 0)
            {
                if (Map.CurrentPosition.X - nearSocket.AbsoluteCoordinate.X > Epsilon)
                    return RobotLocator.GetCommandsByDirection(Direction.Left);
                if (Map.CurrentPosition.X - nearSocket.AbsoluteCoordinate.X < -Epsilon)
                    return RobotLocator.GetCommandsByDirection(Direction.Right);
                if (Map.CurrentPosition.Y - nearSocket.AbsoluteCoordinate.Y > Epsilon)
                    return RobotLocator.GetCommandsByDirection(Direction.Down);
                if (Map.CurrentPosition.Y - nearSocket.AbsoluteCoordinate.Y < -Epsilon)
                    return RobotLocator.GetCommandsByDirection(Direction.Up);
                hasDetail = false;
                return new[] {RTSRules.Current.Grip() };
            }
            return RobotLocator.GetCommandsByDirection(path.First());
        }

        private IEnumerable<MoveAndGripCommand> GripDetail()
        {
            var nearDetail = Map.Details.OrderBy(GetDistance).FirstOrDefault();
            if (nearDetail == null)
                return new MoveAndGripCommand[0];
            color = nearDetail.Type.Split(new[] {"Detail"}, StringSplitOptions.None).First().ToLower();
            var path = PathSearcher.FindPath(Map, OurCoordinates, nearDetail.DiscreteCoordinate);
            hasDetail = path.Length == 0;
            return hasDetail ? new[] { RTSRules.Current.Grip() } : RobotLocator.GetCommandsByDirection(path.First());
        }

        private int GetDistance(StarshipObject starshipObject)
        {
            var objCoordinates = starshipObject.DiscreteCoordinate;
            return Math.Abs(OurCoordinates.X - objCoordinates.X) + Math.Abs(OurCoordinates.Y - objCoordinates.Y);
        }
    }
}
