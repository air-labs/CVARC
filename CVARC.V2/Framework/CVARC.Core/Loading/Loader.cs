using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class Loader
    {
        public Competitions GetCompetitions(LoadingData data)
        {
            throw new NotImplementedException();
        }

        IWorld CreateWorld(Configuration configuration, IRunMode runMode, Competitions competitions)
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
                throw new Exception("Could not load file '" + cmdLineData.Unnamed[0] + "')");
            }
            var configuration = log.Configuration;
            var proposal=SettingsProposal.FromCommandLineData(cmdLineData);
            proposal.Push(configuration.Settings, false, z => z.SpeedUp);
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
            var modeType = (RunModes)Enum.Parse(typeof(RunModes), cmdLineData.Unnamed[2]);
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

        public static string Help = @"
Call with the command line parameters:
CVARC [Competitions] [Level] [Mode] -key1 value1 -key2 value2 ...
or, to play logs:
CVARC [filename] -key1 -value1
or, to set up the network server:
CVARC Debug [PortNumber]

Mode is 'Tutorial' or 'BotDemo'

Keys are:
-EnableLog
-LogFileName [filename]
-Seed [number] to set up random seed for map's generation
-TimeLimit [time in seconds] to limit the time of the round
-OperationalTimeLimit [time in seconds] to limit the time the client works
-SpeedUp to make the simulation faster
-Controller.[ControllerId] [Bot|Client].[Name] sets the controller for robot ControllerId to bot or remote client with the specified name

Examples:
CVARC RepairTheStarship Level1 Tutorial 
CVARC RepairTheStarship Level1 BotDemo -Controller.Left Bot.Azura -Controller.Right Bot.Sanguine
CVARC Debug 14000 
";


    }
}
