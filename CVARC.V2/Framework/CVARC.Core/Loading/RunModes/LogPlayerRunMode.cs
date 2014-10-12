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
            arguments.Pull(log.Configuration, RunModes.Play);
            Configuration = arguments;
        }

        public void InitializeCompetitions(Competitions competitions)
        {
            Competitions = competitions;   
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



        public Competitions Competitions
        {
            get;
            private set; 
        }
    }
}
