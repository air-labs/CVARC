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
    public class NetworkParticipant: Participant
    {
        Competitions competitions;
        public HelloPackage HelloPackage { get; private set; }
        StreamReader clientReader;
        StreamWriter clientWriter;

        public NetworkParticipant(Competitions competitions)
        {
            Random rnd = new Random();

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
                competitions.World.HelloPackage = HelloPackage;
                Console.WriteLine("OK");
            }
            catch (Exception e)
            {
                throw new UserInputException(e);
            }

            switch (HelloPackage.Side)
            {
                case Side.Left: ControlledRobot = 0; break;
                case Side.Right: ControlledRobot = 1; break;
                case Side.Random: ControlledRobot = rnd.Next(2); break;
            }
        }


        override public Controllers.Command MakeTurn()
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

        static string WrapException(Exception exception)
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
            return message;

        }

        public void SendError(Exception exception, bool blameParticipant)
        {
            var message = WrapException(exception);
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
                var message=string.Format("<Result><ExitedWithStatus>{0}</ExitedWithStatus><OperationalTime>{1}</OperationalTime><ExitTime>{2}</ExitTime>",
                    ExitReason,
                    OperationalMilliseconds,
                    ExitTime);
                if (Exception != null)
                    message+="<Exception>" + WrapException(Exception) + "</Exception>";
                if (replayId!=null) message+="<Replay>" + replayId + "</Replay>";
                message += "</Result>";
                clientWriter.WriteLine(message);
                clientWriter.Flush();
            }
        }
    }
}
