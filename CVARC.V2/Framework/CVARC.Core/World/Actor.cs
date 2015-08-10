using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class Actor<TActorManager, TWorld, TCommand, TRules> : IActor
        where TActorManager : IActorManager
        where TWorld : IWorld
        where TCommand : ICommand
        where TRules : IRules
    {
        protected TActorManager Manager { get; private set; }
        public TWorld World { get; private set; }
        IWorld IActor.World { get { return World; } }

        public string ObjectId { get; private set; }
        public Type ExpectedCommandType { get { return typeof(TCommand); } }

        public TRules Rules { get; private set; }
        IRules IActor.Rules { get { return Rules; } }

		public CommandFilterSet FilterSet { get; private set; }

        public void Initialize(IActorManager manager, IWorld world, IRules rules, CommandFilterSet filters, string actorObjectId, string controllerId)
        {
            Manager = Compatibility.Check<TActorManager>(this, manager);
            World = Compatibility.Check<TWorld>(this, world);
            Rules = Compatibility.Check<TRules>(this, rules);
			FilterSet = filters;
            ObjectId = actorObjectId;
            ControllerId = controllerId;
            AdditionalInitialization();
        }

        public virtual void AdditionalInitialization()
        { }



        public string ControllerId
        {
            get;
            private set;
        }

        void IActor.ExecuteCommand(ICommand command, out double duration)
        {
            ExecuteCommand((TCommand)command, out duration);
        }

        public abstract void ExecuteCommand(TCommand command, out double duration);

        public abstract object GetSensorData();
    }
}