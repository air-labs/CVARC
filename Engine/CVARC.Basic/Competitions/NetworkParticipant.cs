using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;
using CVARC.Basic.Controllers;
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

            try
            {
                Console.Write("Receiving hello package... ");
                var line = clientReader.ReadLine();
                var document = XDocument.Parse(line);
                HelloPackage = new HelloPackage();
                HelloPackage.Parse(document);
                Console.WriteLine("OK");
            }
            catch (Exception e)
            {
                throw new UserInputException(e);
            }

            ControlledRobot = HelloPackage.LeftSide ? 0 : 1;
        }

        public Controllers.Command MakeTurn()
        {
            var reply = "<Sensors>";
            foreach (var e in competitions.World.Robots[ControlledRobot].Sensors)
                reply += e.Measure();
            reply += "</Sensors>";
            reply = reply.Replace("\r", "").Replace("\n", "");
            lock (clientWriter)
            {
                clientWriter.WriteLine(reply);
                clientWriter.Flush();
            }

            var request = clientReader.ReadLine();
            Console.WriteLine(request);
            Command command = null;
            try
            {
                command = competitions.NetworkController.ParseRequest(request);
            }
            catch (Exception e)
            {
                throw new UserInputException(e);
            }
            command.RobotId = ControlledRobot;
            return command;
        }

        public void SendError(Exception exception, bool blameParticipant)
        {
            string message = "";
            while (exception != null)
            {
                message += exception.Message + "\n" + exception.StackTrace;
                exception = exception.InnerException;
            }
            message = message.Replace("&", "&amp;");
            message = message.Replace("<", "&lt;");
            message = message.Replace(">", "&gt;");
            message = message.Replace("\n", "<br/>");
            message = message.Replace("\r", "");

            if (blameParticipant) message = "<UserError>" + message + "</UserError>";
            else message = "<SystemError>" + message + "</SystemError>";
            lock (clientWriter)
            {
                clientWriter.WriteLine(message);
                clientWriter.Flush();
            }
            
        }

        public void SendReplay(string replayId)
        {
            lock (clientWriter)
            {
                clientWriter.WriteLine("<Replay>" + replayId + "</Replay>");
                clientWriter.Flush();
            }
        }
    }
}
