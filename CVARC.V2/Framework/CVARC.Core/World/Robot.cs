using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab;

namespace CVARC.V2
{


    public abstract class Robot<TActorManager,TWorld,TSensorsData,TCommand,TRules> :
                    Actor<TActorManager,TWorld, TCommand, TRules>
        where TActorManager : IActorManager
        where TWorld : IWorld
        where TSensorsData : new()
        where TCommand : ICommand
        where TRules : IRules
    {

      

        protected SensorPack<TSensorsData> sensors;

        public abstract IEnumerable<IUnit> Units { get; }

        public override void AdditionalInitialization()
        {
            sensors = new SensorPack<TSensorsData>(this);
        }

        override public object GetSensorData()
        {
            return sensors.MeasureAll();
        }

        public override void ExecuteCommand(TCommand command, out double duration)
        {
            Debugger.Log( DebuggerMessageType.Workflow, "Command comes to robot, " + Units.Count()+" units");
            
            foreach (var e in Units)
            {
                Debugger.Log( DebuggerMessageType.Workflow, "Starting unit "+e.GetType().Name);
                
                var response = e.ProcessCommand(command);
                if (response.Processed)
                {
                    duration = response.RequestedTime;
                    return;
                }
            }
            throw new Exception("The command was not processed by any of the robot units");
        }
    }
}
