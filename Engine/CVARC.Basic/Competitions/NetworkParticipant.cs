using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;
using CVARC.Network;

namespace CVARC.Basic
{
    public class NetworkParticipant: IParticipant
    {
        Competitions competitions;
        public int ControlledRobot { get; private set; }
        public HelloPackage HelloPackage { get; private set; }
        StreamReader clientReader;
        StreamWriter clientWriter;

        public NetworkParticipant(Competitions competitions)
        {
            this.competitions = competitions;
            Console.Write("Starting server... ");
            var listener = new TcpListener(IPAddress.Any, 14000);
            listener.Start();
            Console.WriteLine("OK");


            Console.Write("Waiting for client... ");
            var client = listener.AcceptTcpClient();
            clientReader = new StreamReader(client.GetStream());
            clientWriter = new StreamWriter(client.GetStream());
            Console.WriteLine("OK");

            Console.Write("Receiving hello package... ");
            var line = clientReader.ReadLine();
            var document = XDocument.Parse(line);
            HelloPackage = new HelloPackage();
            HelloPackage.Parse(document);
            Console.WriteLine("OK");

            ControlledRobot = HelloPackage.LeftSide ? 0 : 1;
        }

        public Controllers.Command MakeMove()
        {
            var reply = "<Sensors>";
            foreach (var e in competitions.World.Robots[ControlledRobot].Sensors)
                reply += e.Measure();
            reply += "</Sensors>";
            reply = reply.Replace("\r", "").Replace("\n", "");
            clientWriter.WriteLine(reply);
            clientWriter.Flush();

            var request = clientReader.ReadLine();
            Console.WriteLine(request);
            var command = competitions.NetworkController.ParseRequest(request);
            command.RobotId = ControlledRobot;
            return command;

        }
    }
}
