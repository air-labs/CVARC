using System;
using System.Threading;
using System.Windows.Forms;
using CVARC.Basic;

namespace CVARC.Network
{
    public static class Program
    {
        static NetworkParticipant participant;
        static Participant[] participants;
        static Form form;
        private static bool realTime = true;

        [STAThread]
        static void Main()
        {
            InternalMain(new NetworkSettings());
        }

        public static void DebugMain(NetworkSettings networkSettings)
        {
            InternalMain(networkSettings);
        }

        private static void InternalMain(NetworkSettings settings)
        {
            InitCompetition(settings); //Why CurrentDomain.UnhandledException does not work for this one???
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new TutorialForm(participant.Competitions);
            new Thread(() => participant.Competitions.ProcessParticipants(realTime, 1000, participants))
                {
                    IsBackground = true
                }.Start();
            Application.Run(form);
        }

        private static void InitCompetition(NetworkSettings settings)
        {
            participants = new Participant[2];
            participant = new NetworkParticipant(settings.CompetitionsName);
            participants[participant.ControlledRobot] = participant;
            participant.Competitions.Initialize(new CVARCEngine(participant.Competitions.CvarcRules));
            var botNumber = participant.ControlledRobot == 0 ? 1 : 0;
            var botName = participant.HelloPackage.Opponent ?? "None";
            participants[botNumber] = participant.Competitions.CreateBot(botName, botNumber);
            if (settings.StartClient)
                StartClient();
        }

        private static void StartClient()
        {
            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "..\\..\\..\\..\\Competitions\\DemoNetworkClient\\bin\\Debug\\DemoNetworkClient.exe";
            p.Start();
        }
    }
}
