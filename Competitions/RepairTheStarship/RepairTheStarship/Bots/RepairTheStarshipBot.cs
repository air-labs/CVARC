using System.Collections.Generic;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Basic.Sensors;
using MapHelper;
using RepairTheStarship.Sensors;
using Map = MapHelper.Map;

namespace Gems.Bots
{
    public abstract class RepairTheStarshipBot : Bot
    {
        protected Map Map;
        protected RobotLocator RobotLocator;
        protected Point OpponentCoordinates;
        protected Point OurCoordinates;
        private IEnumerable<Command> currentCommands = new List<Command>();
        private IEnumerator<Command> enumerator;

        public override void Initialize(Competitions competitions)
        {
            base.Initialize(competitions);
            Map = Competitions.GetSensorsData<SensorsData>(ControlledRobot).BuildStaticMap();
            RobotLocator = new RobotLocator(Map);
            enumerator = currentCommands.GetEnumerator();
        }

        public override Command MakeTurn()
        {
            Update();
            if (enumerator.MoveNext())
                return enumerator.Current;
            currentCommands = FindNextCommands();
            enumerator = currentCommands.GetEnumerator();
            return enumerator.MoveNext() ? enumerator.Current : Command.Sleep();
        }

        private void Update()
        {
            RobotLocator.Update(Competitions.GetSensorsData<SensorsData>(ControlledRobot));
            OpponentCoordinates = GetCoordinatesByPosition(Map.OpponentPosition);
            OurCoordinates = GetCoordinatesByPosition(Map.CurrentPosition);
        }

        protected Point GetCoordinatesByPosition(PositionData position)
        {
            return GetCoordinatesByPosition((int)position.X, (int)position.Y);
        }

        protected Point GetCoordinatesByPosition(int positionX, int positionY)
        {
            var point = new Point(positionX, positionY);
            return MapBuilder.AbsoluteCoordinateToDiscrete(point);
        }

        protected abstract IEnumerable<Command> FindNextCommands();
    }
}
