using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CVARC.Basic;
using CVARC.Basic.Core.Participants;

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
                LevelName = LevelName.Level2,
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
                new Thread(() => competitionsBundle.competitions.ProcessParticipants(realTime, 60 * 1000, participants))
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
            var participantsServer = new ParticipantsServer(competitionsName);
            var participantsTask = Task.Factory.StartNew(() => participantsServer.GetParticipants(helloPackage));
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
}
