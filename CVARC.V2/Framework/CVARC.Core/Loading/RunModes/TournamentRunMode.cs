using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CVARC.V2
{
    class TournamentRunMode : StandardRunMode
    {
        TcpListener tcpServer;
        List<Process> processes = new List<Process>();
        List<TcpClient> clients = new List<TcpClient>();

        public override void Initialize(Configuration configuration, Competitions competitions)
        {
            base.Initialize(configuration, competitions);
            tcpServer = new System.Net.Sockets.TcpListener(configuration.Settings.Port);
            tcpServer.Start();
            Competitions.Logic.World.Exit += World_Exit;
        }

        void World_Exit()
        {
            foreach (var client in clients)
                client.Close();
            foreach (var process in processes)
                if (!process.HasExited)
                    process.Kill();
        }

        public override IController GetController(string controllerId)
        {
            var config = this.GetControllerConfigFor(controllerId);
            if (config.Type == ControllerType.Bot) return this.GetBotFor(config);
            var solutionDirectory = string.Format("{0}\\{1}",Configuration.Settings.SolutionsFolder,config.Name); 
            if (!Directory.Exists(solutionDirectory))
                throw new Exception("The solution directory '"+solutionDirectory+"' was not found");
            var solutionFileName = solutionDirectory + "\\run.bat";
            if (!File.Exists(solutionFileName))
                throw new Exception("The run.bat file was not found in the directory '"+solutionDirectory+"'");

            var process = new Process
            {
                StartInfo =
                {
                    FileName = "run.bat",
                    Arguments = "release",
                    WorkingDirectory=solutionDirectory
                }
            };
            process.EnableRaisingEvents = false;
            process.Start();
            processes.Add(process);
            Thread.Sleep(1000);
            var client = tcpServer.AcceptTcpClient();
            clients.Add(client);
            var cvarcClient = new CvarcTcpClient(client);
            var controller = new NetworkController(cvarcClient);
            controller.ReadConfiguration();
            controller.OperationalTimeLimit = Configuration.Settings.OperationalTimeLimit;
            return controller;
        }
    }
}
