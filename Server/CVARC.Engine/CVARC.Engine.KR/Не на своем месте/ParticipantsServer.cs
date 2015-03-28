using System;
using System.Net;
using System.Net.Sockets;
using CVARC.Basic.Core.Serialization;
using CVARC.Network;

namespace CVARC.Basic.Core.Participants
{
    public class ParticipantsServer : IDisposable
    {
        private readonly string competitionsName;
        private readonly TcpListener listener;
        private CompetitionsBundle competitionsBundle;
        public ICvarcRules Rules { get { return competitionsBundle.Rules; } }
        public Competitions Competitions { get { return competitionsBundle.Competitions; } }

        public ParticipantsServer(string competitionsName)
        {
            this.competitionsName = competitionsName;
            listener = new TcpListener(IPAddress.Any, 14000);
            listener.Start();
        }

        public NetworkParticipant GetParticipant(HelloPackage helloPackage = null)
        {
            var client = new ClientWithPackage(listener.AcceptTcpClient());
            competitionsBundle = GetCompetitionsBundle(helloPackage ?? client.HelloPackage);
            int controlledRobot = competitionsBundle.Competitions.HelloPackage.Side == Side.Random ? new Random().Next(2) : (int)competitionsBundle.Competitions.HelloPackage.Side;
            return new NetworkParticipant(competitionsBundle.Competitions, controlledRobot, client.Client);
        }

        public NetworkParticipant[] GetParticipants(HelloPackage helloPackage)
        {
            var client = new ClientWithPackage(listener.AcceptTcpClient());
            var client2 = new ClientWithPackage(listener.AcceptTcpClient());
            competitionsBundle = GetCompetitionsBundle(helloPackage);
            return new[]
            {
                new NetworkParticipant(competitionsBundle.Competitions, 0, client.Client), 
                new NetworkParticipant(competitionsBundle.Competitions, 1, client2.Client)
            };
        }

        private CompetitionsBundle GetCompetitionsBundle(HelloPackage helloPackage)
        {
            var bundle = CompetitionsBundle.Load(competitionsName, helloPackage.LevelName.ToString());
            bundle.Competitions.HelloPackage = helloPackage;
            return bundle;
        }

        private class ClientWithPackage
        {
            private static readonly ISerializer Serializer = new JsonSerializer();
            public HelloPackage HelloPackage { get; set; }
            public GroboTcpClient Client { get; set; }

            public ClientWithPackage(TcpClient client)
            {
                Client = new GroboTcpClient(client);
                HelloPackage = Serializer.Deserialize<HelloPackage>(Client.ReadToEnd());
            }
        }

        public void Dispose()
        {
            listener.Stop();
        }
    }
}