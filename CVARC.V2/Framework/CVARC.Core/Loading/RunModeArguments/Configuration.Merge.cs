using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    
    partial class Configuration
    {
        public void Pull(Configuration Configuration, RunModes puller)
        {
            if (puller == RunModes.Play)
            {
                this.Mode = RunModes.Play;

                Assembly = Configuration.Assembly;
                EnableLog = false;
                Level = Configuration.Level;
                LogFile = null;
                Seed = Configuration.Seed;
                TimeLimit = Configuration.TimeLimit;
            }

            if (puller == RunModes.Debug)
            {
                this.Mode = RunModes.Debug;

                this.Assembly = Configuration.Assembly;
                this.EnableLog = Configuration.EnableLog;
                this.Level = Configuration.Level;
                this.LogFile = Configuration.LogFile;
                
                this.OperationalTimeLimit = Configuration.OperationalTimeLimit;
                this.Port = Configuration.Port;
                this.Seed = Configuration.Seed;
                this.SpeedUp = Configuration.SpeedUp;
                this.TimeLimit = Configuration.TimeLimit;


            }

        }
    }
}
