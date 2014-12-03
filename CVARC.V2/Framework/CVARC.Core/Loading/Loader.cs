using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CVARC.V2
{



    public class Loader
    {
        #region Stored levels 

        public readonly Dictionary<string, Dictionary<string, Func<Competitions>>> Levels = new Dictionary<string, Dictionary<string, Func<Competitions>>>();

        public void AddLevel(string competitions, string level, Func<Competitions> factory)
        {
            if (!Levels.ContainsKey(competitions))
                Levels[competitions] = new Dictionary<string, Func<Competitions>>();
            Levels[competitions][level] = factory;
        }

        public Competitions GetCompetitions(string assemblyName, string level)
        {
            return Levels[assemblyName][level]();
        }
        
        public Competitions GetCompetitions(LoadingData data)
        {
            return GetCompetitions(data.AssemblyName, data.Level);
        }
        
        #endregion

        #region Non-networking modes

        public IWorld CreateWorld(Configuration configuration, ControllerFactory controllerFactory, IWorldState state)
        {
            var competitions = GetCompetitions(configuration.LoadingData);
            var world = competitions.Logic.CreateWorld();
            world.Initialize(competitions, configuration, controllerFactory, state);
            return world;
        }

        public IWorld CreateLogPlayer(CommandLineData cmdLineData)
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
            var proposal = SettingsProposal.FromCommandLineData(cmdLineData);
            proposal.Push(configuration.Settings, false, z => z.SpeedUp);
            configuration.Settings.EnableLog = false;
            configuration.Settings.LogFile = null;
            var factory = new LogPlayerControllerFactory(log);
            return CreateWorld(configuration, factory, log.WorldState);
        }

		public ControllerFactory CreateControllerFactory(string modeName)
		{
			ControllerFactory factory = null;
			if (modeName  == "BotDemo")
				factory = new BotDemoControllerFactory();
			else if (modeName == "Tutorial")
				factory = new TutorialControllerFactory();
			else throw new Exception("Mode '" + modeName + "' is unknown");
			return factory;
		}
        public IWorld CreateSimpleMode(CommandLineData cmdLineData)
        {
			ControllerFactory factory = CreateControllerFactory(cmdLineData.Unnamed[2]);
			var proposal = SettingsProposal.FromCommandLineData(cmdLineData);
            var loadingData = new LoadingData { AssemblyName = cmdLineData.Unnamed[0], Level = cmdLineData.Unnamed[1] };
			return CreateSimpleMode(loadingData, proposal, factory);
		}

		 public IWorld CreateSimpleMode(LoadingData loadingData, SettingsProposal proposal, ControllerFactory factory)
		 {
            var configuration = new Configuration();
            configuration.LoadingData = loadingData;
            var competitions = GetCompetitions(configuration.LoadingData);
            configuration.Settings=competitions.Logic.GetDefaultSettings();
			 

            proposal.Push(configuration.Settings,true);
            var stateName = configuration.Settings.WorldState;
            if (stateName == null)
            {
                if (competitions.Logic.PredefinedStatesNames.Count == 0)
                    throw new Exception("The count of predefined stated in the " + competitions.Logic.GetType() + " is zero");
                stateName = competitions.Logic.PredefinedStatesNames[0];
            }
            var state = competitions.Logic.CreatePredefinedState(stateName);
            return CreateWorld(configuration, factory, state);

        }

        #endregion

        #region SoloNetwork

        public const int DefaultPort = 14000;

        public void InstantiateWorld(NetworkServerData data)
        {
            var factory = new SoloNetworkControllerFactory(data.ClientOnServerSide);
            var configuration = new Configuration
            {
                 LoadingData=data.LoadingData,
                 Settings=data.Settings
            };
            data.World = CreateWorld(configuration, factory, data.WorldState);
            data.World.Exit += () =>
                {
                    data.Close();
                };
        }


        public void ReceiveConfiguration(NetworkServerData data)
        {
            var configProposal = data.ClientOnServerSide.Read<ConfigurationProposal>();
            data.LoadingData = configProposal.LoadingData;
            var competitions = GetCompetitions(data.LoadingData);
            data.Settings = competitions.Logic.GetDefaultSettings();
            if (configProposal.SettingsProposal != null)
                configProposal.SettingsProposal.Push(data.Settings, true);
            var worldSettingsType = competitions.Logic.GetWorldStateType();
            data.WorldState = (IWorldState)data.ClientOnServerSide.Read(worldSettingsType);
        }

        public void RunServer(NetworkServerData data)
        {
            var server = new System.Net.Sockets.TcpListener(data.Port);
            server.Start();
            data.ServerLoaded = true;
            var client = server.AcceptTcpClient();
            data.ClientOnServerSide = new CvarcClient(client);
            data.StopServer = () =>
                {
                    client.Close();
                    server.Stop();
                };
        }

        public void CreateSoloNetworkWithData(NetworkServerData nsdata)
        {
            RunServer(nsdata);
            ReceiveConfiguration(nsdata);
            InstantiateWorld(nsdata);
        }

        public IWorld CreateSoloNetwork(CommandLineData data)
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
                port = DefaultPort;
            var nsdata = new NetworkServerData();
            nsdata.Port = port;
            CreateSoloNetworkWithData(nsdata);
            return nsdata.World;
        }

    

        #endregion

        #region SelfTest

        void SelfTestClientThread(ICvarcTest test, IAsserter asserter, NetworkServerData holder)
        {
            holder.WaitForServer();
            test.Run(holder, asserter);
        }


        public ICvarcTest GetTest(LoadingData data, string testName)
        {
            var assemblyName = data.AssemblyName;
            var level = data.Level;
            Competitions competitions;
            try
            {
                competitions = GetCompetitions(assemblyName, level);
            }
            catch
            {
                throw new Exception(string.Format("The competition '{0}'.'{1}' were not found", assemblyName, level));
            }
            ICvarcTest test;
            try
            {
                test = competitions.Logic.Tests[testName];
            }
            catch
            {
                throw new Exception(string.Format("The test with name '{0}' was not found in competitions {1}.{2}", testName, assemblyName, level));
            }
            return test;
        }

        public void CreateSelfTestServer(NetworkServerData holder, SettingsProposal proposal)
        {
            RunServer(holder);
            ReceiveConfiguration(holder);
            proposal.Push(holder.Settings, true);
            InstantiateWorld(holder);
        }

        public IWorld CreateSelfTestInCommandLineContext(CommandLineData data, IAsserter asserter)
        {
            var holder = new NetworkServerData();
            holder.Port=DefaultPort;
            holder.LoadingData = new LoadingData { AssemblyName=data.Unnamed[0], Level=data.Unnamed[1] };
            var test = GetTest(holder.LoadingData, data.Unnamed[3]);
            new Action<ICvarcTest, IAsserter, NetworkServerData>(SelfTestClientThread).BeginInvoke(test, asserter, holder, null, null);
            var proposal = SettingsProposal.FromCommandLineData(data);
            CreateSelfTestServer(holder,proposal);
            return holder.World;
        }

        public void RunSelfTestInVSContext(string assemblyName, string level, string testName, IAsserter asserter)
        {
            var holder = new NetworkServerData();
            holder.Port = DefaultPort;
            holder.LoadingData = new LoadingData { AssemblyName = assemblyName, Level = level };
            var test = GetTest(holder.LoadingData, testName);

            var thread = new Thread(() =>
            {
                var proposal = new SettingsProposal { SpeedUp = true };
                CreateSelfTestServer(holder, proposal);
                holder.World.RunActively(1);
                holder.Close();
            }) { IsBackground = true };
            thread.Start();

            try
            {
                SelfTestClientThread(test, asserter, holder);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                holder.Close();
            }
        }

        #endregion

        public IWorld Load(string[] arguments)
        {
            var cmdLineData = CommandLineData.Parse(arguments);
            
            if (cmdLineData.Unnamed.Count==0)
                throw new Exception("CVARC required parameters to run. See manual");

            if (cmdLineData.Unnamed[0] == "Debug")
                return CreateSoloNetwork(cmdLineData);
            else if (cmdLineData.Unnamed.Count == 1)
                return CreateLogPlayer(cmdLineData);
            else if (cmdLineData.Unnamed.Count == 4 && cmdLineData.Unnamed[2] == "SelfTest")
                return CreateSelfTestInCommandLineContext(cmdLineData, new EmptyAsserter());
            else
                return CreateSimpleMode(cmdLineData);
        }

        public static string Help = @"

";


    }
}
