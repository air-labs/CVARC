using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class ControllerFactory
    {
        public IWorld World;
        
        public virtual void Initialize(IWorld world)
        {
            this.World = world;
        }

        protected ControllerSettings GetSettings(string controllerId)
        {
            var result = World.Configuration.Settings.Controllers.Where(z => z.ControllerId == controllerId).FirstOrDefault();
            if (result == null)
                throw new Exception("The controller '" + controllerId + "' is not defined in settings");
            return result;
        }

        protected IController CreateBot(string controllerId)
        {
            var sets = GetSettings(controllerId);
            if (sets.Type != ControllerType.Bot)
                throw new Exception("Internal error: trying to create bot for '" + controllerId + "', but settings define '" + sets.Type + "'");
            var botName=sets.Name;
            if (!World.Competitions.Logic.Bots.ContainsKey(botName))
                throw new Exception("Bot '"+botName+"' is not defined");
            return World.Competitions.Logic.Bots[botName]();
        }


        public abstract IController Create(string controllerId, IActor actor);

		public virtual void Exit() { }
    }

}
