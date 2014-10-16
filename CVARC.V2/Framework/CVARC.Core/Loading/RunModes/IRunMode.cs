using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public interface IRunMode
    {
        void CheckArguments(Configuration arguments);
        void InitializeCompetitions(Competitions competitions);
        IController GetController(string controllerId);
        Configuration Configuration { get; }
        Competitions Competitions { get; }
    }


    public static class IRunModeExtensions
    {
        public static ControllerSettings GetControllerConfigFor(this IRunMode mode, string controllerId)
        {
            var record = mode.Configuration.Controllers.Where(z => z.ControllerId == controllerId).FirstOrDefault();
            if (record == null) 
                throw new Exception(string.Format("The controller '{0}' is not specified by the configuration", controllerId));
            return record;
        }

        public static IController GetBotFor(this IRunMode m, ControllerSettings record)
        {
            if (!m.Competitions.Logic.Bots.ContainsKey(record.Name))
                throw new Exception(string.Format("The bot '{0}' specified for controller '{1}' is not defined", record.Name, record.ControllerId));
            var bot = m.Competitions.Logic.Bots[record.Name]();
            return bot;
        }

    }


}