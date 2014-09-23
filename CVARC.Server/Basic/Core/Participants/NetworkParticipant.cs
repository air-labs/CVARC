using System.Text;
using CVARC.Basic.Controllers;
using CVARC.Basic.Core.Serialization;
using CVARC.Network;

namespace CVARC.Basic.Core.Participants
{
    public class NetworkParticipant : Participant
    {
        private CompetitionsBundle CompetitionsBundle { get; set; }
        private static readonly ISerializer Serializer = new JsonSerializer();
        private readonly GroboTcpClient client;

        public NetworkParticipant(CompetitionsBundle competitionsBundle, int controlledRobot, GroboTcpClient client)
        {
            CompetitionsBundle = competitionsBundle;
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
            var sensorsData = CompetitionsBundle.competitions.GetSensorsData<ISensorsData>(ControlledRobot);
            client.Send(Serializer.Serialize(sensorsData));
            var command = Serializer.Deserialize<Command>(client.ReadToEnd());
            command.RobotId = ControlledRobot;
            return command;
        }
    }
}