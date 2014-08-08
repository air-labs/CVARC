using System.Collections.Generic;
using System.Linq;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Basic.Sensors;
using MapHelper;
using RepairTheStarship.Sensors;
using Map = MapHelper.Map;

namespace RepairTheStarship.Bots
{
    class MolagBal : FixedProgramBot
    {
        private Map map;
        private RobotLocator robotLocator;
        private IEnumerable<Command> currentCommands = new List<Command>();
        private IEnumerator<Command> enumerator;

        public override void Initialize(Competitions competitions)
        {
            base.Initialize(competitions);
            map = Competitions.GetSensorsData<SensorsData>(ControlledRobot).BuildStaticMap();
            robotLocator = new RobotLocator(map);
            enumerator = currentCommands.GetEnumerator();
        }

        public override Command MakeTurn()
        {
            robotLocator.Update(Competitions.GetSensorsData<SensorsData>(ControlledRobot));
            if (enumerator.MoveNext())
                return enumerator.Current;
            currentCommands = FindNextCommands();
            enumerator = currentCommands.GetEnumerator();
            return enumerator.MoveNext() ? enumerator.Current : Command.Sleep();
        }

        private IEnumerable<Command> FindNextCommands()
        {
            var opponentCoordinates = GetCoordinatesByPosition(map.OpponentPosition);
            var botCoordinates = GetCoordinatesByPosition(map.CurrentPosition);
            var path = PathSearcher.FindPath(map, botCoordinates, opponentCoordinates);
            return path.Length == 0 ? new Command[0] : robotLocator.GetCommandsByDirection(path.First());
        }

        private Point GetCoordinatesByPosition(PositionData position)
        {
            var point = new Point((int)position.X, (int)position.Y);
            return MapBuilder.AbsoluteCoordinateToDiscrete(point);
        }
    }
}
