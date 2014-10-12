//using System;
//using System.Net;
//using System.Net.Sockets;
//using CVARC.Basic.Core.Serialization;
//using CVARC.Network;

//namespace CVARC.Basic.Core.Participants
//{
//    public class ParticipantsServer
//    {
//        private readonly string competitionsName;
//        private readonly TcpListener listener;
//        public CompetitionsBundle CompetitionsBundle { get; set; }

//        public ParticipantsServer(string competitionsName)
//        {
//            this.competitionsName = competitionsName;
//            listener = new TcpListener(IPAddress.Any, 14000);
//            listener.Start();
//        }

//        public NetworkParticipant GetParticipant()
//        {
//            var client = new ClientWithPackage(listener.AcceptTcpClient());
//            CompetitionsBundle = GetCompetitionsBundle(client.HelloPackage);
//            int controlledRobot = client.HelloPackage.Side == Side.Random ? new Random().Next(2) : (int)client.HelloPackage.Side;
//            return new NetworkParticipant(CompetitionsBundle.competitions, controlledRobot, client.Client);
//        }

//        public NetworkParticipant[] GetParticipants(HelloPackage helloPackage)
//        {
//            var client = new ClientWithPackage(listener.AcceptTcpClient());
//            var client2 = new ClientWithPackage(listener.AcceptTcpClient());
//            CompetitionsBundle = GetCompetitionsBundle(helloPackage);
//            return new[]
//            {
//                new NetworkParticipant(CompetitionsBundle.competitions, 0, client.Client), 
//                new NetworkParticipant(CompetitionsBundle.competitions, 1, client2.Client)
//            };
//        }

//        private CompetitionsBundle GetCompetitionsBundle(HelloPackage helloPackage)
//        {
//            var competitionsBundle = CompetitionsBundle.Load(competitionsName, helloPackage.LevelName);
//            competitionsBundle.competitions.HelloPackage = helloPackage;
//            return competitionsBundle;
//        }

//        private class ClientWithPackage
//        {
//            private static readonly ISerializer Serializer = new JsonSerializer();
//            public HelloPackage HelloPackage { get; set; }
//            public GroboTcpClient Client { get; set; }
            
//            public ClientWithPackage(TcpClient client)
//            {
//                Client = new GroboTcpClient(client);
//                HelloPackage = Serializer.Deserialize<HelloPackage>(Client.ReadToEnd());
//            }
//        }
//    }
//}