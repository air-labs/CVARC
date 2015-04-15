using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CVARC.V2
{
	public class TournamentControllerFactory : ControllerFactory
	{
		readonly int Port;
		readonly TcpListener listener;
		const int timeToEstablishConnection = 1000;
		List<CvarcClient> clients = new List<CvarcClient>();
		List<Process> processes = new List<Process>();
		public TournamentControllerFactory(int port)
		{
			Port=port;
			listener = new TcpListener(Port);
			listener.Start();
		}

		public override IController Create(string controllerId, IActor actor)
		{
			// if bot is assigned for controller, get it 
			var settings = GetSettings(controllerId);
			if (settings.Type == ControllerType.Bot)
				return CreateBot(controllerId);
			
			
			// running process
			var process = new Process();
			var directory = "Solutions\\" + settings.Name ;
			if (!Directory.Exists(directory))
				throw new Exception("Directory "+directory+" for robot "+controllerId+" is not found");
			var batFile = directory+"\\run.bat";
			if (!File.Exists(batFile))
				throw new Exception("run.bat is not found in directory "+directory+" for robot "+controllerId);
			process.StartInfo.FileName = "run.bat";
			process.StartInfo.WorkingDirectory = directory;
			process.StartInfo.Arguments = Port.ToString();
			process.Start();

			//waiting for incoming connection
			var watch = new Stopwatch();
			watch.Start();
			while(!listener.Pending())
			{
				if (watch.ElapsedMilliseconds>1000)
				{
					watch.Stop();
					if (!process.HasExited) process.Kill();
					throw new Exception("Process " + batFile + " for controller " + controllerId + " haven't established connection for "+timeToEstablishConnection+" millisecond");
				}
			}
			watch.Stop();

			processes.Add(process);
			var socketClient = listener.AcceptTcpClient();
			var client = new CvarcClient(socketClient);
			clients.Add(client);

			// skipping config so the client's protocol won't change from SoloNetwork mode
			client.Read<ConfigurationProposal>();
			client.Read(World.Competitions.Logic.WorldStateType);

			var controller = World.Competitions.Logic.Actors[controllerId].CreateNetworkController();
			controller.InitializeClient(client);
			return controller;
		}

		public override void Exit()
		{
			foreach (var client in clients)
				client.Close();
			listener.Stop();
			foreach (var process in processes)
				if (!process.HasExited)
					process.Kill();
		}
	}
}
