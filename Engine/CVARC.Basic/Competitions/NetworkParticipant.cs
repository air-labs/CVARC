using System;
using System.IO;
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
        Competitions competitions;
        public HelloPackage HelloPackage { get; private set; }
        StreamReader clientReader;
        StreamWriter clientWriter;
        private ISerializer serializer = new JsonSerializer();
        private NetworkStream stream;

        public NetworkParticipant(Competitions competitions)
        {
            this.competitions = competitions;
            Console.Write("Starting server... ");
            var listener = new TcpListener(IPAddress.Any, 14000);
            listener.Start();
            Console.WriteLine("OK");

            Console.Write("Waiting for client... ");
            var client = listener.AcceptTcpClient();
            stream = client.GetStream();
            clientReader = new StreamReader(stream);
            clientWriter = new StreamWriter(stream);
            Console.WriteLine("OK");

            try
            {
                var package = stream.ReadBytes();
                Console.Write("Receiving hello package... {0}", Encoding.UTF8.GetString(package));
                HelloPackage = serializer.Deserialize<HelloPackage>(package);
                competitions.World.HelloPackage = HelloPackage;
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

        override public Command MakeTurn()
        {
            var sensorsData = competitions.GetSensorsData(ControlledRobot);
            lock (clientWriter)
            {
                stream.Write(serializer.Serialize(sensorsData));
                stream.Flush();
            }

            var request = clientReader.ReadLine();
            Console.WriteLine(request);
            Command command = competitions.NetworkController.ParseRequest(request);
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
