using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab;
using CVARC.Basic;
using CVARC.Basic.Core.Participants;

namespace CVARC.V2
{
    public abstract class Robot<TActorManager,TWorld,TSensorsData,TCommand> : Actor<TActorManager,TWorld, TCommand>
        where TActorManager : IActorManager
        where TWorld : IWorld
        where TSensorsData : new()
        where TCommand : ICommand
    {

        public Robot(string controlledId)
        {
            ControllerId = controlledId;
        }

        SensorPack<TSensorsData> sensors;

        public override void Initialize(IActorManager rules, IWorld world, string actorObjectId)
        {
            base.Initialize(rules, world, actorObjectId);
            sensors = new SensorPack<TSensorsData>(this);
        }

        override public object GetSensorData()
        {
            return sensors.MeasureAll();
        }
    }
}
