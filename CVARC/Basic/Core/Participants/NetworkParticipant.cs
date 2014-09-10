using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CVARC.Basic.Controllers;
using CVARC.Basic.Core;
using CVARC.Basic.Core.Serialization;
using CVARC.Network;

namespace CVARC.Basic
{
    public class NetworkParticipant: Participant
    {
        public CompetitionsBundle CompetitionsBundle { get; private set; }
        public HelloPackage HelloPackage { get; private set; }
        private readonly ISerializer serializer = new JsonSerializer();
        private readonly NetworkStream stream;

        public NetworkParticipant(string competitionsName)
        {
            Console.Write("Starting server... ");
            var listener = new TcpListener(IPAddress.Any, 14000);
            listener.Start();
            Console.WriteLine("OK");

            Console.Write("Waiting for client... ");
            var client = listener.AcceptTcpClient();
            stream = client.GetStream();
            Console.WriteLine("OK");

            try
            {
                var package = stream.ReadBytes();
                Console.Write("Receiving hello package... {0}", Encoding.UTF8.GetString(package));
                HelloPackage = serializer.Deserialize<HelloPackage>(package);
                CompetitionsBundle = CompetitionsBundle.Load(competitionsName, HelloPackage.LevelName);
                CompetitionsBundle.Competitions.HelloPackage = HelloPackage;
            }
            catch (Exception e)
            {
                throw new UserInputException(e);
            }

            switch (HelloPackage.Side)
            {
                case Side.Left: ControlledRobot = 0; break;
                case Side.Right: ControlledRobot = 1; break;
                case Side.Random: ControlledRobot = new Random().Next(2); break;
            }
        }

        public override Command MakeTurn()
        {
            var sensorsData = CompetitionsBundle.Competitions.GetSensorsData<ISensorsData>(ControlledRobot);
            stream.Write(serializer.Serialize(sensorsData));
            stream.Flush();

            var command = serializer.Deserialize<Command>(stream.ReadBytes());
            command.RobotId = ControlledRobot;
            return command;
        }
    }
}
