using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Basic.Core.Serialization;

namespace CVARC.Network
{
    public static class Program
    {
        private static CompetitionsBundle competitionsBundle;
        static Participant[] participants;
        static Form form;
        private static bool realTime = true;

        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, a) =>
            {
                MessageBox.Show(a.ExceptionObject.ToString(), "CVARC Network", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            };
            InternalMain(new HelloPackage
            {
                LevelName = "Level2",
            }, "Fall2013.0.dll");
        }

        private static void InternalMain(HelloPackage helloPackage, string competitionsName)
        {
            try
            {
                InitCompetition(helloPackage, competitionsName);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                form = new TutorialForm(competitionsBundle.competitions);
                new Thread(() => competitionsBundle.competitions.ProcessParticipants(realTime, 1000, participants))
                {
                    IsBackground = true
                }.Start();
                Application.Run(form);
            }
            catch (Exception e)
            {
                throw;//todo Убрать
                MessageBox.Show(e.Message, "CVARC Network", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        private static void InitCompetition(HelloPackage helloPackage, string competitionsName)
        {
            var participantsServer = new ParticipantsServer(helloPackage, competitionsName);
            var participantsTask = Task.Factory.StartNew(() => participantsServer.GetParticipants());
            RunClients("player2\\Client.exe", "player1\\Client.exe");
            participants = participantsTask.Result;
            competitionsBundle = participantsServer.CompetitionsBundle;
            participantsServer.CompetitionsBundle.competitions.Initialize(new CVARCEngine(competitionsBundle.Rules), new[]
            {
                new RobotSettings(0, false), 
                new RobotSettings(1, false)
            });
        }

        private static void RunClients(string firstPlayer, string secondPlayer)
        {
            Process.Start("..\\..\\participants\\" + firstPlayer, "noRunServer");
            Thread.Sleep(300);
            Process.Start("..\\..\\participants\\" + secondPlayer, "noRunServer");
        }
    }

    public class ParticipantsServer
    {
        public CompetitionsBundle CompetitionsBundle { get; set; }
        private readonly TcpListener listener;

        public ParticipantsServer(HelloPackage helloPackage, string competitionsName)
        {
            CompetitionsBundle = CompetitionsBundle.Load(competitionsName, helloPackage.LevelName);
            CompetitionsBundle.competitions.HelloPackage = helloPackage;
            listener = new TcpListener(IPAddress.Any, 14000);
            listener.Start();
        }

        public QParticipant[] GetParticipants()
        {
            var client = new GroboTcpClient(listener.AcceptTcpClient());
            client.ReadToEnd();
            var client2 = new GroboTcpClient(listener.AcceptTcpClient());
            client2.ReadToEnd();
            return new[]
            {
                new QParticipant(CompetitionsBundle, 0, client), 
                new QParticipant(CompetitionsBundle, 1, client2)
            };
        }
    }

    public class QParticipant : Participant
    {
        private CompetitionsBundle CompetitionsBundle { get; set; }
        private readonly ISerializer serializer = new JsonSerializer();
        private readonly GroboTcpClient client;

        public QParticipant(CompetitionsBundle competitionsBundle, int controlledRobot, GroboTcpClient client)
        {
            CompetitionsBundle = competitionsBundle;
            ControlledRobot = controlledRobot;
            this.client = client;
        }

        public override Command MakeTurn()
        {
            var sensorsData = CompetitionsBundle.competitions.GetSensorsData<ISensorsData>(ControlledRobot);
            client.Send(serializer.Serialize(sensorsData));
            var command = serializer.Deserialize<Command>(client.ReadToEnd());
            command.RobotId = ControlledRobot;
            return command;
        }
    }
}
