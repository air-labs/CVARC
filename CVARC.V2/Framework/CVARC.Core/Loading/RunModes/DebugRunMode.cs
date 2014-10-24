﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CVARC.V2
{
    public class DebugRunMode : IRunMode
    {
        int givenControllers = 0;
        NetworkController controller;
        ConfigurationProposal proposal;
        TcpClient client;

        public DebugRunMode(int portNumber)
        {
            var tcpServer = new System.Net.Sockets.TcpListener(portNumber);
            tcpServer.Start();
            client = tcpServer.AcceptTcpClient();
            var grobo = new CvarcTcpClient(client);
            controller = new NetworkController(grobo);
            proposal = controller.ReadConfiguration();
        }

        public ConfigurationProposal GetConfigurationProposal()
        {
            return proposal;
        }

        public void Initialize(Configuration configuration, Competitions competitions)
        {
            this.Configuration = configuration; 
            this.Competitions = competitions;
            controller.OperationalTimeLimit = configuration.Settings.OperationalTimeLimit;
            competitions.Logic.World.Exit += World_Exit;
        }

        void World_Exit()
        {
            client.Close();
        }

        public IController GetController(string controllerId)
        {
            var record = this.GetControllerConfigFor(controllerId);
            if (record.Type == ControllerType.Bot)
                return this.GetBotFor(record);
            if (givenControllers != 0)
                throw new Exception("Only one network controller can be assigned in this mode");
            givenControllers++;
            return controller;
        }




        public Configuration Configuration
        {
            get;
            private set; 
        }

        public Competitions Competitions
        {
            get;
            private set; 
        }
    }
}