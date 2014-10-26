using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class LogPlayRunMode : IRunMode
    {

        Log log;
        Configuration configuration;

        public LogPlayRunMode(Log log)
        {
            this.log = log;
        }


        public void Initialize(IWorld world, Configuration configuration, Competitions competitions)
        {
            Configuration = configuration;
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
