using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	/// <summary>
	/// This part of the class creates a world in the case when no network interaction is required.
	/// </summary>
	partial class Loader
	{

		/// <summary>
		/// Creates a world. 
		/// </summary>
		/// <param name="configuration">Contains the cometition/level name and the settings</param>
		/// <param name="controllerFactory">Is used to create controllers, i.e. entities that control the robots</param>
		/// <param name="state">The initial state of the world</param>
		/// <returns></returns>
		public IWorld CreateWorld(Configuration configuration, ControllerFactory controllerFactory, IWorldState state)
		{
			var competitions = GetCompetitions(configuration.LoadingData);
			var world = competitions.Logic.CreateWorld();
			world.Initialize(competitions, configuration, controllerFactory, state);
			world.Exit += ()=>
				controllerFactory.Exit();
			return world;
		}

		/// <summary>
		/// Creates a world that will play the log file. Controllers, settings and state will be obtained from the log file. 
		/// </summary>
		/// <param name="cmdLineData"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Creates a controller factory by the name of selected mode. 
		/// </summary>
		/// <param name="modeName">BotDemo or Tutorial</param>
		/// <returns></returns>
		public ControllerFactory CreateControllerFactory(string modeName, SettingsProposal proposal)
		{
			ControllerFactory factory = null;
			if (modeName == "BotDemo")
				factory = new BotDemoControllerFactory();
			else if (modeName == "Tutorial")
				factory = new TutorialControllerFactory();
			else if (modeName  == "Tournament")
			{
				var port = 14000;
				if (proposal.Port.HasValue) port = proposal.Port.Value;
				factory = new TournamentControllerFactory(port);
			}
			else throw new Exception("Mode '" + modeName + "' is unknown");
			return factory;
		}


		/// <summary>
		/// Creates the world for non-networking case: BotDemo or Tutorial modes.
		/// </summary>
		/// <param name="loadingData"></param>
		/// <param name="proposal"></param>
		/// <param name="factory"></param>
		/// <returns></returns>
		public IWorld CreateSimpleMode(LoadingData loadingData, SettingsProposal proposal, ControllerFactory factory)
		{
			var configuration = new Configuration();
			configuration.LoadingData = loadingData;
			var competitions = GetCompetitions(configuration.LoadingData);
			configuration.Settings = competitions.Logic.CreateDefaultSettings();


			proposal.Push(configuration.Settings, true);
			var stateName = configuration.Settings.WorldState;
			if (stateName == null)
			{
				if (competitions.Logic.PredefinedWorldStates.Count == 0)
					throw new Exception("The count of predefined stated in the " + competitions.Logic.GetType() + " is zero");
				stateName = competitions.Logic.PredefinedWorldStates[0];
			}
			var state = competitions.Logic.CreateWorldState(stateName);
			return CreateWorld(configuration, factory, state);

		}

		/// <summary>
		/// Creates the world for non-networking case: BotDemo or Tutorial modes. Automatically obtains all the parameters from CommandLineData
		/// </summary>
		/// <param name="cmdLineData"></param>
		/// <returns></returns>
		public IWorld CreateSimpleMode(CommandLineData cmdLineData)
		{
			var proposal = SettingsProposal.FromCommandLineData(cmdLineData);
			ControllerFactory factory = CreateControllerFactory(cmdLineData.Unnamed[2], proposal);
			var loadingData = new LoadingData { AssemblyName = cmdLineData.Unnamed[0], Level = cmdLineData.Unnamed[1] };
			return CreateSimpleMode(loadingData, proposal, factory);
		}


	}
}
