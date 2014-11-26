using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class ControllerRequest
    {
        public readonly Competitions Competitions;
        public readonly Settings Settings;
        public readonly string ControllerId;
        public readonly ControllerSettings ControllerSettings;
        public ControllerRequest(Competitions competitions, Settings settings, string controllerId)
        {
            Competitions = competitions;
            Settings = settings;
            ControllerId = controllerId;
            ControllerSettings = Settings.Controllers.Where(z => z.ControllerId == ControllerId).FirstOrDefault();
            if (ControllerSettings == null)
                throw new Exception("The controller '" + ControllerId + "' is not defined in settings");
        }
        public IController CreateBot()
        {
            if (ControllerSettings.Type != ControllerType.Bot)
                throw new Exception("Internal error: trying to create bot for '" + ControllerId + "', but settings define '" + ControllerSettings.Type + "'");
            var botName=ControllerSettings.Name;
            if (!Competitions.Logic.Bots.ContainsKey(botName))
                throw new Exception("Bot '"+botName+"' is not defined");
            return Competitions.Logic.Bots[botName]();
        }
    }

    public interface IControllerFactory
    {
        IController Create(ControllerRequest request);
    }

}
