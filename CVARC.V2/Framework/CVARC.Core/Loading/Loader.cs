﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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

        public Competitions GetCompetitions(string assemblyName, string level)
        {
            return Levels[assemblyName][level]();
        }
        public Competitions GetCompetitions(LoadingData data)
        {
            return GetCompetitions(data.AssemblyName, data.Level);   
        }

        public IWorld CreateWorld(Configuration configuration, IRunMode runMode, Competitions competitions)
        {
            var world = competitions.Logic.CreateWorld(); 
            runMode.Initialize(world, configuration, competitions);
            world.Initialize(competitions, runMode);
            return world;
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


        public IWorld LoadNonLogFile(IRunMode mode, LoadingData loadingData, SettingsProposal proposal)
        {
            var configuration = new Configuration();
            configuration.LoadingData = loadingData;
            var competitions = GetCompetitions(loadingData);
            configuration.Settings = competitions.Logic.GetDefaultSettings();
            proposal.Push(configuration.Settings, true);
            return CreateWorld(configuration, mode, competitions);
        }

        public IWorld LoadFromNetwork(IMessagingClient client)
        {
            var mode = new DebugRunMode(client);
            var configProposal = mode.GetConfigurationProposal();
            return LoadNonLogFile(mode, configProposal.LoadingData, configProposal.SettingsProposal);
       
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

            var tcpServer = new System.Net.Sockets.TcpListener(port);
            tcpServer.Start();
            var client = tcpServer.AcceptTcpClient();
            var messagingClient = new CvarcTcpClient(client);

            return LoadFromNetwork(messagingClient);
        }

        void SelfTestThread(ICvarcTest test, int port, IAsserter asserter, TestAsyncLock holder)
        {
            holder.WaitForServer();
            test.Run(port, holder, asserter);
        }

        public IWorld LoadSelfTest(string assemblyName, string level, string testName, IAsserter asserter)
        {
            int port=14000;
            var holder = new TestAsyncLock();
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
                test=competitions.Logic.Tests[testName];
            }
            catch
            {
                throw new Exception(string.Format("The test with name '{0}' was not found in competitions {1}.{2}",testName,assemblyName,level));
            }
            new Action<ICvarcTest, int, IAsserter, TestAsyncLock>(SelfTestThread).BeginInvoke(test, port, asserter, holder, null, null);
            

            var tcpServer = new System.Net.Sockets.TcpListener(port);
            tcpServer.Start();
            holder.ServerLoaded = true;
            
            var client = tcpServer.AcceptTcpClient();
            var messagingClient = new CvarcTcpClient(client);
            var world = LoadFromNetwork(messagingClient);
            holder.World=world;
            return world;
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
            else if (cmdLineData.Unnamed.Count == 4 && cmdLineData.Unnamed[2] == "SelfTest")
                return LoadSelfTest(cmdLineData.Unnamed[0], cmdLineData.Unnamed[1], cmdLineData.Unnamed[3], new EmptyAsserter());
            else
                return LoadNormally(cmdLineData);
        }

        public static string Help = @"

";


    }
}