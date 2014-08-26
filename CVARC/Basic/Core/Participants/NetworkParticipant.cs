using System;
using System.Net;
using System.Net.Sockets;
using CVARC.Basic.Controllers;
using CVARC.Basic.Core.Serialization;
using CVARC.Network;

namespace CVARC.Basic
{
    public class NetworkParticipant: Participant
    {
        public Competitions Competitions { get; private set; }
        public HelloPackage HelloPackage { get; private set; }
        private readonly ISerializer serializer = new JsonSerializer();
        private readonly GroboTcpClient client;

        public NetworkParticipant(string competitionsName)
        {
            var listener = new TcpListener(IPAddress.Any, 14000);
            listener.Start();
            client = new GroboTcpClient(listener.AcceptTcpClient());

            try
            {
                var package = client.ReadToEnd();
                HelloPackage = serializer.Deserialize<HelloPackage>(package);
                Competitions = Competitions.Load(competitionsName, HelloPackage.LevelName);
                Competitions.HelloPackage = HelloPackage;
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
            var sensorsData = Competitions.GetSensorsData<ISensorsData>(ControlledRobot);
            client.Send(serializer.Serialize(sensorsData));
            var command = serializer.Deserialize<Command>(client.ReadToEnd());
            command.RobotId = ControlledRobot;
            return command;
        }
    }
}
