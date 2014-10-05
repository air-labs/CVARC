using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic.Core.Participants;
using CVARC.Core;

namespace CVARC.V2
{


    public abstract class World<TSceneState,TWorldManager> : IWorld
        where TWorldManager : IWorldManager
    {
        List<IActor> actors;

        public TSceneState SceneSettings { get; private set; }
        public IEngine Engine { get; private set; }
        public TWorldManager Manager { get; private set; }
        IWorldManager IWorld.Manager { get { return Manager; } }
        public IdGenerator IdGenerator { get; private set; }
        public WorldClocks Clocks { get; private set; }

        protected abstract IEnumerable<IActor> CreateActors();

        public void Initialize(Competitions competitions, IEnvironment environment)
        {
            Clocks = new WorldClocks();
            IdGenerator = new IdGenerator();

            //Initializing world
            this.SceneSettings = (TSceneState)environment.GetSceneSettings();
            this.Engine = competitions.Engine.Engine;
            this.Manager = (TWorldManager)competitions.Manager.WorldManager;
            Engine.Initialize(this);
            Manager.Initialize(this);
            Manager.CreateWorld(IdGenerator);


            //Initializing actors
            actors = CreateActors().ToList();
            foreach (var e in actors)
            {
                var actorObjectId = IdGenerator.CreateNewId(e);
                var manager = competitions.Manager.CreateActorManagerFor(e);
                e.Initialize(manager, this, actorObjectId);
                manager.Initialize(e);
                manager.CreateActorBody();
            }


            foreach (var e in actors.OfType<IControllable>())
                e.AcceptParticipant(environment.GetController(e.ControllerId));
        }


    }
}
