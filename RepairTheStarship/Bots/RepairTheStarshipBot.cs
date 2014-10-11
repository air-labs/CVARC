using System.Collections.Generic;
using CVARC.V2;
using CVARC.V2.SimpleMovement;
using RepairTheStarship.MapBuilder;
using Map = RepairTheStarship.MapBuilder.Map;

namespace RepairTheStarship.Bots
{
    public abstract class RepairTheStarshipBot : Controller<SimpleMovementCommand>
    {
        SensorPack<BotsSensorsData> sensors;
        protected Map Map;
        protected RobotLocator RobotLocator;
        protected Point OpponentCoordinates;
        protected Point OurCoordinates;
        private IEnumerable<SimpleMovementCommand> currentCommands = new List<SimpleMovementCommand>();
        private IEnumerator<SimpleMovementCommand> enumerator;
        protected RTSWorld world;

        override public SimpleMovementCommand GetCommand()
        {
            Update();
            if (enumerator.MoveNext())
                return enumerator.Current;
            currentCommands = FindNextCommands();
            enumerator = currentCommands.GetEnumerator();
            return enumerator.MoveNext() ? enumerator.Current : world.ExitCommand();
        }

        override public void Initialize(IActor controllableActor)
        {
            world = (RTSWorld)controllableActor.World;
            sensors = new SensorPack<BotsSensorsData>(controllableActor);
            Map = sensors.MeasureAll().BuildMap();
            RobotLocator = new RobotLocator(Map,world);
            enumerator = currentCommands.GetEnumerator();
        }


        private void Update()
        {
            RobotLocator.Update(sensors.MeasureAll());
            OpponentCoordinates = Map.GetDiscretePosition(Map.OpponentPosition);
            OurCoordinates = Map.GetDiscretePosition(Map.CurrentPosition);
        }

        protected abstract IEnumerable<SimpleMovementCommand> FindNextCommands();


    }
}
