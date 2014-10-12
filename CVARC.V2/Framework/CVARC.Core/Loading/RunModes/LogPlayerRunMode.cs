using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class LogPlayRunMode : IRunMode
    {
        Log log;

        public void CheckArguments(Configuration arguments)
        {
            log = Log.Load(arguments.LogFile);
            arguments.EnableLog = false;
            arguments.LogFile = null;
            arguments.Assembly = log.Configuration.Assembly;
            arguments.Level = log.Configuration.Level;
            arguments.Seed = log.Configuration.Seed;
            arguments.TimeLimit = log.Configuration.TimeLimit;
            Configuration = arguments;
        }

        public void InitializeCompetitions(Competitions competitions)
        {
            
        }

        public IController GetController(string controllerId)
        {
            return new LogPlayController(log.Commands[controllerId]);
        }

        public Configuration Configuration
        {
            get;
            private set; 
        }
    }
}
