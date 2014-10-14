using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class DebugRunMode : IRunMode
    {
        int givenControllers = 0;
        NetworkController controller;

        public void CheckArguments(Configuration arguments)
        {
            var tcpServer = new System.Net.Sockets.TcpListener(arguments.Port);
            tcpServer.Start();
            var client = tcpServer.AcceptTcpClient();
            var grobo = new CvarcTcpClient(client);
            controller = new NetworkController(grobo);
            arguments.Pull(controller.ReadConfiguration(), RunModes.Debug);
            Configuration = arguments;
        }

        public void InitializeCompetitions(Competitions competitions)
        {
            Competitions = competitions;   
        }

        public IController GetController(string controllerId)
        {
            var record = this.GetControllerConfigFor(controllerId);
            if (record.Type == ControllerType.Bot)
                return this.GetBotFor(record);
            if (givenControllers != 0)
                throw new Exception("Only one network controller can be assigned in this mode");
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
