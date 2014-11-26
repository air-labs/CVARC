
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{


    public abstract class World<TWorldManager> : IWorld
        where TWorldManager : IWorldManager
    {
        List<IActor> actors;

        public IEngine Engine { get; private set; }
        public TWorldManager Manager { get; private set; }
        IWorldManager IWorld.Manager { get { return Manager; } }
        public IdGenerator IdGenerator { get; private set; }
        public WorldClocks Clocks { get; private set; }
        public Scores Scores { get; private set; }
        public Logger Logger { get; private set; }
        public Configuration Configuration { get; private set; }
        public Competitions Competitions { get; private set; }

        public abstract void CreateWorld();

        public void OnExit()
        {
            if (Exit != null) Exit();
        }

        public event Action Exit;

        public IEnumerable<IActor> Actors
        {
            get { return actors; }
        }
        

        public virtual void Initialize(Competitions competitions, Configuration configuration, ControllerFactory controllerFactory)
        {

            Competitions = competitions;
            Configuration = configuration;
            Clocks = new WorldClocks();
            IdGenerator = new IdGenerator();
            Scores = new Scores(this);
            Logger = new Logger(this);

            // setting up the parameters
            Logger.SaveLog = Configuration.Settings.EnableLog;

            Logger.LogFileName = Configuration.Settings.LogFile;
            Logger.Log.Configuration = Configuration;

            Clocks.TimeLimit = Configuration.Settings.TimeLimit;
                 

            


            //Initializing world
            this.Engine = competitions.Engine.EngineFactory();  
            this.Manager = Compatibility.Check<TWorldManager>(this,competitions.Manager.WorldManagerFactory());
            Engine.Initialize(this);
            Manager.Initialize(this);
            controllerFactory.Initialize(this);
            CreateWorld();


            //Initializing actors
            actors = new List<IActor>();
            foreach(var id in competitions.Logic.ControllersId)
            {
                var e = competitions.Logic.CreateActor(id);
                var actorObjectId = IdGenerator.CreateNewId(e);
                var manager = competitions.Manager.CreateActorManagerFor(e);
                e.Initialize(manager, this, actorObjectId, id);
                manager.Initialize(e);
                manager.CreateActorBody();
                var controller = controllerFactory.Create(e.ControllerId);
                controller.Initialize(e);
                var preprocessor = competitions.Logic.CreateCommandPreprocessor(id);
                preprocessor.Initialize(e);
                Clocks.AddTrigger(new ControlTrigger(controller, e, preprocessor));
                actors.Add(e);
            }
        }
    }
}
