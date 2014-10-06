using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab;
using CVARC.Basic;
using CVARC.Basic.Core.Participants;

namespace CVARC.V2
{
    public abstract class Robot<TActorManager,TWorld,TSensorsData,TCommand> : Actor<TActorManager,TWorld>, IControllable
        where TActorManager : IActorManager
        where TWorld : IWorld
        where TSensorsData : new()
        where TCommand : ICommand
    {
        public int ClockdownTime = -1;

        IController<TCommand> controller;

        public Robot(string controlledId)
        {
            ControllerId = controlledId;
        }

        SensorPack<TSensorsData> sensors;

        public override void Initialize(IActorManager rules, IWorld world, string actorObjectId)
        {
            base.Initialize(rules, world, actorObjectId);
            World.Clocks.SetClockdown(0, PerformControl);
            sensors = new SensorPack<TSensorsData>(this);
        }

        public string ControllerId
        {
            get;
            private set; 
        }

        public void AcceptParticipant(IController controller)
        {
            try
            {
                this.controller = controller as IController<TCommand>;
            }
            catch
            {
                throw new Exception("The controller " + controller.GetType() + " cannot be used with robot " + GetType() + " due to TCommand incompatibility");
            }
        }

        protected abstract void ProcessCommand(TCommand command, out double nextRequestTimeSpan);
        void PerformControl(ClockdownData trigger, out double NextScheduledTime)
        {
            double nextRequestTimeSpan;
            ProcessCommand(controller.GetCommand(), out nextRequestTimeSpan);
            NextScheduledTime = trigger.ThisCallTime + nextRequestTimeSpan;
        }

    }
}
