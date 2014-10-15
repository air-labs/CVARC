
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public IRunMode RunMode { get; private set; }

        public abstract IEnumerable<string> ControllersId { get; }
        public abstract IActor CreateActor(string controllerId);
        public abstract TSceneState CreateSceneState(int seed);

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
            Logger.SaveLog = environment.Configuration.EnableLog;
            Logger.LogFileName = environment.Configuration.LogFile;
            Logger.Log.Configuration = environment.Configuration;

            if (environment.Configuration.TimeLimit.HasValue)
                Clocks.TimeLimit = environment.Configuration.TimeLimit.Value;
            else
                Clocks.TimeLimit = competitions.Logic.TimeLimit;
                 
            

            //Initializing world
            this.SceneSettings = CreateSceneState(environment.Configuration.Seed);
            this.Engine = competitions.Engine.Engine;
            this.Manager = (TWorldManager)competitions.Manager.WorldManager;
            Engine.Initialize(this);
            Manager.Initialize(this);
            Manager.CreateWorld(IdGenerator);


            //Initializing actors
            actors = new List<IActor>();
            foreach(var id in ControllersId)
            {
                var e=CreateActor(id);
                var actorObjectId = IdGenerator.CreateNewId(e);
                var manager = competitions.Manager.CreateActorManagerFor(e);
                e.Initialize(manager, this, actorObjectId);
                manager.Initialize(e);
                manager.CreateActorBody();
                var controller = environment.GetController(e.ControllerId);
                controller.Initialize(e);
                Clocks.AddTrigger(new ControlTrigger(controller, e));
                actors.Add(e);
            }
        }



    }
}
