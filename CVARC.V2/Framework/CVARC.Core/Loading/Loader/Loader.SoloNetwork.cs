using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	/// <summary>
	/// This part of the class allows running the competitions AND the network server, in case the server is required
	/// 
	/// Most methods of this class accepts and modifies the NetworkServerData, which store all the information about client-server interaction.
	/// </summary>
	partial class Loader
	{
		public const int DefaultPort = 14000;

		/// <summary>
		/// Creates a controller for the network client and instantiate the world with this controller. This action is the last before the start.
		/// </summary>
		/// <param name="data"></param>
		public void InstantiateWorld(NetworkServerData data)
		{
			var factory = new SoloNetworkControllerFactory(data.ClientOnServerSide);
			var configuration = new Configuration
			{
				LoadingData = data.LoadingData,
				Settings = data.Settings
			};
			data.World = CreateWorld(configuration, factory, data.WorldState);
			data.World.Exit += () =>
			{
				if (data != null)
					data.Close();
			};
		}

		/// <summary>
		/// This method receives the configuration and the world's initial state from the client.
		/// </summary>
		/// <param name="data"></param>
		public void ReceiveConfiguration(NetworkServerData data)
		{
			var configProposal = data.ClientOnServerSide.Read<ConfigurationProposal>();
			data.LoadingData = configProposal.LoadingData;
			var competitions = GetCompetitions(data.LoadingData);
			data.Settings = competitions.Logic.CreateDefaultSettings();
			if (configProposal.SettingsProposal != null)
				configProposal.SettingsProposal.Push(data.Settings, true);
			var worldSettingsType = competitions.Logic.WorldStateType;
			data.WorldState = (IWorldState)data.ClientOnServerSide.Read(worldSettingsType);
		}

		/// <summary>
		/// This method runs TCP/IP listener, waits for the connected client and set the closing actions for client and server after the competitions are over.
		/// </summary>
		/// <param name="data"></param>
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

		/// <summary>
		/// Completely initializes the world. The NetworkServerData should only contain the port.
		/// </summary>
		/// <param name="nsdata"></param>
		public void CreateSoloNetworkWithData(NetworkServerData nsdata)
		{
			RunServer(nsdata);
			ReceiveConfiguration(nsdata);
			InstantiateWorld(nsdata);
		}

		/// <summary>
		/// Creates world for a solo network mode from a command line arguments.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
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

    
	}
}
