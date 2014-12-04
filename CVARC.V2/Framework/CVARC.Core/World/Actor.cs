using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class Actor<TActorManager, TWorld, TCommand> : IActor
        where TActorManager : IActorManager
        where TWorld : IWorld
        where TCommand : ICommand
    {
        protected TActorManager Manager { get; private set; }
        public TWorld World { get; private set; }
        IWorld IActor.World { get { return World; } }

        public string ObjectId { get; private set; }
        public Type ExpectedCommandType { get { return typeof(TCommand); } }

        public void Initialize(IActorManager rules, IWorld world, string actorObjectId, string controllerId)
        {
            Manager = Compatibility.Check<TActorManager>(this, rules);
            World = Compatibility.Check<TWorld>(this, world);
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

        void IActor.ExecuteCommand(ICommand command)
        {
            ExecuteCommand((TCommand)command);
        }

        public abstract void ExecuteCommand(TCommand command);

        public abstract object GetSensorData();
    }
}