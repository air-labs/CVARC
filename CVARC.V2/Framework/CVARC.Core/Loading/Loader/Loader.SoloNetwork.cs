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

    
	}
}
