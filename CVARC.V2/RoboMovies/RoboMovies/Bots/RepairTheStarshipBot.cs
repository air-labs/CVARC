using System.Collections.Generic;
using CVARC.V2;
using CVARC.V2;
using RoboMovies.MapBuilder;
using Map = RoboMovies.MapBuilder.InternalMap;

namespace RoboMovies.Bots
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
        protected RMWorld world;

        override public MoveAndGripCommand GetCommand()
        {
            Update();
            if (enumerator.MoveNext())
                return enumerator.Current;
            currentCommands = FindNextCommands();
            enumerator = currentCommands.GetEnumerator();
            return enumerator.MoveNext() ? enumerator.Current : RMRules.Current.Stand(1);
        }

        override public void Initialize(IActor controllableActor)
        {
            world = (RMWorld)controllableActor.World;
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
