using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic.Core.Participants;

namespace CVARC.V2
{
    public interface IWorld 
    {
        Engine Engine { get; }
        void Tick(double time);
    }

    public abstract class World<TSceneState,TWorldManager> : IWorld
    {
        List<IActor> actors;

        public TSceneState SceneState { get; private set; }
        public Engine Engine { get; private set; }
        public TWorldManager Manager { get; private set; }
        public event Action<double> Triggers;

        public void Tick(double time)
        {
            if (Triggers != null) Triggers(time);
            foreach (var e in actors)
                e.Tick(time);
            Engine.Physical.Tick(time);
        }

        protected abstract IEnumerable<IActor> CreateActors();

        public void Initialize(TSceneState sceneState, Engine engine, IEnumerable<IController> controllers)
        {
            //Initializing world
            this.SceneState = sceneState;
            this.Engine = engine;
            this.Manager = (TWorldManager)engine.WorldManager;
            engine.Physical.Initialize(this);
            engine.WorldManager.Initialize(this);
            engine.WorldManager.CreateWorld(Engine.IdGenerator);


            //Initializing actors
            actors = CreateActors().ToList();
            foreach(var e in actors)
            {
                var actorObjectId = Engine.IdGenerator.CreateNewId(e);
                var rules=engine.ActorManagerFactories.Where(z=>z.ActorManagerType==e.GetManagerType).FirstOrDefault();
                IActorManager manager = null;
                if (rules != null)
                    manager = rules.Generate();
                e.Initialize(manager,this, actorObjectId);
                if (manager != null)
                {
                    manager.Initialize(e);
                    manager.CreateActorBody();
                }
            }



            //Initializing controllable actors
            var controllable = actors.OfType<IControllable>().ToArray();
            var used=new HashSet<int>();
            foreach (var e in controllable)
            {
                var number = e.ControllerNumber;
                if (used.Contains(number))
                    throw new Exception("The controller number " + number + " is used more than once");
                var controller=controllers.Where(z=>z.ControllerNumber==number).FirstOrDefault();
                if (controller==null)
                    throw new Exception("The controller number " + number + " is not found in controllers pool");
                controller.Initialize(this);
                e.AcceptParticipant(controller);
                used.Add(number);
            }
            var unusedControllers=controllers
                .Select(z=>z.ControllerNumber)
                .Where(z=>!used.Contains(z))
                .ToArray();
            if (unusedControllers.Length != 0)
            {
                var unused = unusedControllers.Select(z => z.ToString()).Aggregate((a, b) => a + " " + b);
                throw new Exception("The controller numbers " + unused + " were unused");
            }
        }
    }
}
