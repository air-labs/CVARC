using System;
using System.Text;
using CVARC.Basic.Controllers;
using CVARC.Basic.Core.Serialization;
using CVARC.Network;

namespace CVARC.Basic.Core.Participants
{
    public class NetworkParticipant : Participant
    {
        private Competitions Competitions { get; set; }
        private static readonly ISerializer Serializer = new JsonSerializer();
        private readonly GroboTcpClient client;

        public NetworkParticipant(Competitions competitionsBundle, int controlledRobot, GroboTcpClient client)
        {
            Competitions = competitionsBundle;
            ControlledRobot = controlledRobot;
            this.client = client;
            SendSide(controlledRobot);
        }

        private void SendSide(int side)
        {
            client.Send(Encoding.UTF8.GetBytes(((Side)side).ToString()));
        }

        public override Command MakeTurn()
        {
            try
            {
                var sensorsData = Competitions.GetSensorsData<ISensorsData>(ControlledRobot);
                client.Send(Serializer.Serialize(sensorsData));
                var command = Serializer.Deserialize<Command>(client.ReadToEnd());
                command.RobotId = ControlledRobot;
                return command;
            }
            catch (Exception)
            {
                return Command.Sleep();
            }
        }
    }
}