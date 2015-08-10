
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{


    public abstract class World<TWorldState, TWorldManager> : IWorld
        where TWorldManager : IWorldManager
        where TWorldState : IWorldState
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
        public TWorldState WorldState { get; private set; }
		IWorldState IWorld.WorldState { get { return WorldState;  } }
        public IKeyboard Keyboard { get; private set; }
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

        public virtual void AdditionalInitialization()
        {
        }


        public void Initialize(Competitions competitions, Configuration configuration, ControllerFactory controllerFactory, IWorldState worldState)
        {
            Debugger.Log(DebuggerMessageType.Initialization, "World initialization");
            Debugger.Log(DebuggerMessageType.Initialization, "Starting basic fields");
            Competitions = competitions;
            Configuration = configuration;
            WorldState = Compatibility.Check<TWorldState>(this, worldState);

            Clocks = new WorldClocks();
            IdGenerator = new IdGenerator();
            Scores = new Scores(this);
            Logger = new Logger(this);
            Keyboard = competitions.Engine.KeyboardFactory();

            // setting up the parameters
            Logger.SaveLog = Configuration.Settings.EnableLog;

            Logger.LogFileName = Configuration.Settings.LogFile;
            Logger.Log.Configuration = Configuration;
            Logger.Log.WorldState = WorldState;

            Clocks.TimeLimit = Configuration.Settings.TimeLimit;



            //Initializing world
            this.Engine = competitions.Engine.EngineFactory();
            this.Manager = Compatibility.Check<TWorldManager>(this, competitions.Manager.WorldManagerFactory());

            Debugger.Log(DebuggerMessageType.Initialization, "Complete: basic fields. Starting engine");
            Engine.Initialize(this);
            Debugger.Log(DebuggerMessageType.Initialization, "Complete: engine. Starting world manager");
            Manager.Initialize(this);
            Debugger.Log(DebuggerMessageType.Initialization, "Complete: world manager. Starting controller factory");
            controllerFactory.Initialize(this);
            Debugger.Log(DebuggerMessageType.Initialization, "Complete: controller factory. Creating world");
            CreateWorld();
            Debugger.Log(DebuggerMessageType.Initialization, "World created");
            

            //Initializing actors
            actors = new List<IActor>();
            foreach (var id in competitions.Logic.Actors.Keys)
            {
                Debugger.Log(DebuggerMessageType.Initialization, "Actor "+id+" initialization");
                Debugger.Log(DebuggerMessageType.Initialization, "Creating actor");
                var factory = competitions.Logic.Actors[id];
                var e = factory.CreateActor();
                var actorObjectId = IdGenerator.CreateNewId(e);
                Debugger.Log(DebuggerMessageType.Initialization, "Complete: actor. Creating manager");
                var manager = competitions.Manager.CreateActorManagerFor(e);
                var rules = factory.CreateRules();
				var preprocessor = factory.CreateCommandFilterSet();
                e.Initialize(manager, this, rules, preprocessor, actorObjectId, id);

                Debugger.Log(DebuggerMessageType.Initialization, "Comlete: manager creation. Initializing manager");
                manager.Initialize(e);
                Debugger.Log(DebuggerMessageType.Initialization, "Comlete: manager initialization. Creating actor body");
                manager.CreateActorBody();

                Debugger.Log(DebuggerMessageType.Initialization, "Complete: body. Starting controller");
                
                var controller = controllerFactory.Create(e.ControllerId, e);
                controller.Initialize(e);

                Clocks.AddTrigger(new ControlTrigger(controller, e, preprocessor));
                actors.Add(e);
                Debugger.Log(DebuggerMessageType.Initialization, "Actor "+id+" is initialized");   
            }

            Debugger.Log(DebuggerMessageType.Initialization, "Additional world initialization");
            AdditionalInitialization();
        }



        public void RunActively(double requiredPhysicalDelta)
        {
            bool stop = false;
            Exit += () => stop = true;
            var oldTime = 0.0;
            while (!stop)
            {
                var time = Clocks.GetNextEventTime();
                if (time >= Configuration.Settings.TimeLimit)
                    time = Configuration.Settings.TimeLimit;

                if (time - oldTime > requiredPhysicalDelta)
                    time = oldTime + requiredPhysicalDelta;
                if (Engine is IPassiveEngine) (Engine as IPassiveEngine).Update(oldTime, time);
                Clocks.Tick(time);
                oldTime = time;

                if (time == Configuration.Settings.TimeLimit)
                    break;
            }
            OnExit();
        }
    }
}
