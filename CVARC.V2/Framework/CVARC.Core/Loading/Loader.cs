using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class Loader
    {
        public readonly Dictionary<string, Dictionary<string, Func<Competitions>>> Levels = new Dictionary<string, Dictionary<string, Func<Competitions>>>();

        public void AddLevel(string competitions, string level, Func<Competitions> factory)
        {
            if (!Levels.ContainsKey(competitions))
                Levels[competitions] = new Dictionary<string, Func<Competitions>>();
            Levels[competitions][level] = factory;
        }


        public Competitions GetCompetitions(LoadingData data)
        {
            return Levels[data.AssemblyName][data.Level]();
        }

        public IWorld CreateWorld(Configuration configuration, IRunMode runMode, Competitions competitions)
        {
            runMode.Initialize(configuration, competitions);
            competitions.Logic.World.Initialize(competitions, runMode);
            return competitions.Logic.World;
        }

        IWorld LoadFromLogFile(CommandLineData cmdLineData)
        {

            Log log;
            try
            {
                log = Log.Load(cmdLineData.Unnamed[0]);
            }
            catch
            {
                throw new Exception("Could not load file '" + cmdLineData.Unnamed[0] + "'");
            }
            var configuration = log.Configuration;
            var proposal=SettingsProposal.FromCommandLineData(cmdLineData);
            proposal.Push(configuration.Settings, false, z => z.SpeedUp);
            configuration.Settings.EnableLog = false;
            configuration.Settings.LogFile = null;
            var mode = new LogPlayRunMode(log);
            var competitions = GetCompetitions(configuration.LoadingData);
            return CreateWorld(configuration, mode, competitions);
        }


        IWorld LoadNonLogFile(IRunMode mode, LoadingData loadingData, SettingsProposal proposal)
        {
            var configuration = new Configuration();
            configuration.LoadingData = loadingData;
            var competitions = GetCompetitions(loadingData);
            configuration.Settings = competitions.Logic.GetDefaultSettings();
            proposal.Push(configuration.Settings, true);
            return CreateWorld(configuration, mode, competitions);
        }

        IWorld LoadFromNetwork(CommandLineData data)
        {
            int port;
            if (data.Unnamed.Count > 1)
            {
                try
                {
                    port = int.Parse(data.Unnamed[1]);
                }
                catch
                {
                    throw new Exception("Port number '" + data.Unnamed[1] + "' is incorrect: integer expected");
                }
            }
            else
                port = 14000;
            var mode = new DebugRunMode(port);
            var configProposal = mode.GetConfigurationProposal();
            return LoadNonLogFile(mode, configProposal.LoadingData, configProposal.SettingsProposal);
        }

        IWorld LoadNormally(CommandLineData cmdLineData)
        {
            var loadingData = new LoadingData();
            loadingData.AssemblyName = cmdLineData.Unnamed[0];
            loadingData.Level = cmdLineData.Unnamed[1];
            RunModes modeType;
            try
            {
                modeType = (RunModes)Enum.Parse(typeof(RunModes), cmdLineData.Unnamed[2]);
            }
            catch
            {
                throw new Exception("The mode '" + cmdLineData.Unnamed[2] + "' is unknown");
            }
            var mode = RunModeFactory.Create(modeType);
            var proposal = SettingsProposal.FromCommandLineData(cmdLineData);
            return LoadNonLogFile(mode, loadingData, proposal);
        }

        public IWorld Load(string[] arguments)
        {
            var cmdLineData = CommandLineData.Parse(arguments);
            
            if (cmdLineData.Unnamed.Count==0)
                throw new Exception("CVARC required parameters to run. See manual");

            if (cmdLineData.Unnamed[0] == "Debug")
                return LoadFromNetwork(cmdLineData);
            else if (cmdLineData.Unnamed.Count == 1)
                return LoadFromLogFile(cmdLineData);
            else 
                return LoadNormally(cmdLineData);
        }
    }
}
