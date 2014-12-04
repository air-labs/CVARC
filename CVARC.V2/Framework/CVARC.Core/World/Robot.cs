using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab;

namespace CVARC.V2
{
    public abstract class Robot<TActorManager,TWorld,TSensorsData,TCommand> : Actor<TActorManager,TWorld, TCommand>
        where TActorManager : IActorManager
        where TWorld : IWorld
        where TSensorsData : new()
        where TCommand : ICommand
    {

      

        SensorPack<TSensorsData> sensors;

        public override void AdditionalInitialization()
        {
            sensors = new SensorPack<TSensorsData>(this);
        }

        override public object GetSensorData()
        {
            return sensors.MeasureAll();
        }
    }
}
