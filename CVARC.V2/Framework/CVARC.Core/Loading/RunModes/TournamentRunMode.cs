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

        public override void Initialize(Configuration configuration, Competitions competitions)
        {
            base.Initialize(configuration, competitions);
            tcpServer = new System.Net.Sockets.TcpListener(configuration.Settings.Port);
            tcpServer.Start();
        }

        public override IController GetController(string controllerId)
        {
            var config = this.GetControllerConfigFor(controllerId);
            if (config.Type == ControllerType.Bot) return this.GetBotFor(config);
            
            var solutionFileName = string.Format("{0}\\{1}\\run.bat -release",Configuration.Settings.SolutionsFolder, config.Name);
            if (!File.Exists(solutionFileName))
                throw new Exception("The file with the solution was not found at the path '"+solutionFileName+"'");

            var process = new Process
            {
                StartInfo =
                {
                    FileName = solutionFileName,
                    Arguments = "release"
                }
            };
            process.Start();
            Thread.Sleep(1000);
            var client = tcpServer.AcceptTcpClient();
            var cvarcClient = new CvarcTcpClient(client);
            var controller = new NetworkController(cvarcClient);
            controller.ReadConfiguration();
            controller.OperationalTimeLimit = Configuration.Settings.OperationalTimeLimit;
            return controller;
        }
    }
}
