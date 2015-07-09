using System.Collections.Generic;
using CVARC.V2;
using CVARC.V2;
using RepairTheStarship.MapBuilder;
using Map = RepairTheStarship.MapBuilder.InternalMap;

namespace RepairTheStarship.Bots
{
    public abstract class RepairTheStarshipBot : Controller<MoveAndGripCommand>
    {
        SensorPack<BotsSensorsData> sensors;
        protected InternalMap Map;
        protected RobotLocator RobotLocator;
        protected Point OpponentCoordinates;
        protected Point OurCoordinates;
        private IEnumerable<MoveAndGripCommand> currentCommands = new List<MoveAndGripCommand>();
        private IEnumerator<MoveAndGripCommand> enumerator;
        protected RTSWorld world;

        override public MoveAndGripCommand GetCommand()
        {
            Update();
            if (enumerator.MoveNext())
                return enumerator.Current;
            currentCommands = FindNextCommands();
            enumerator = currentCommands.GetEnumerator();
            return enumerator.MoveNext() ? enumerator.Current : RTSRules.Current.Stand(1);
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

        protected abstract IEnumerable<MoveAndGripCommand> FindNextCommands();


    }
}
