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
        public Scores Scores { get; private set; }
        public Logger Logger { get; private set; }
        protected abstract IEnumerable<IActor> CreateActors();
        public IRunMode RunMode { get; private set; }

        public void OnExit()
        {
            if (Exit != null) Exit();
        }

        public event Action Exit;

        public IEnumerable<IActor> Actors
        {
            get { return actors; }
        }

        public virtual void Initialize(Competitions competitions, IRunMode environment)
        {
            RunMode = environment;
            Clocks = new WorldClocks();
            IdGenerator = new IdGenerator();
            Scores = new Scores(this);
            Logger = new Logger(this);

            // setting up the parameters
            Logger.SaveLog = environment.Arguments.EnableLog;
            Logger.LogFileName = environment.Arguments.LogFile;

            if (environment.Arguments.TimeLimit.HasValue)
                Clocks.TimeLimit = environment.Arguments.TimeLimit.Value;
            else
                Clocks.TimeLimit = competitions.Logic.TimeLimit;
                 
            

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
            {
                var controller = environment.GetController(e.ControllerId);
                controller.Initialize(e);
                e.AcceptParticipant(controller);
            }
        }



    }
}
