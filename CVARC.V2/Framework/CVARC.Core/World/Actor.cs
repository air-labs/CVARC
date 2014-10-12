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

        public virtual void Initialize(IActorManager rules, IWorld world, string actorObjectId)
        {
            try
            {
                Manager = (TActorManager)rules;
            }
            catch { throw new Exception("This should not happen, because rules are checked to match this robot"); }
            try
            {
                World = (TWorld)world;
            }
            catch { throw new Exception("The actor " + GetType().Name + " is not designed for world " + world.GetType().Name); }

            ObjectId = actorObjectId;
        }




        public string ControllerId
        {
            get;
            protected set;
        }

        void IActor.ExecuteCommand(ICommand command)
        {
            ExecuteCommand((TCommand)command);
        }

        public abstract void ExecuteCommand(TCommand command);

        public abstract object GetSensorData();
    }
}