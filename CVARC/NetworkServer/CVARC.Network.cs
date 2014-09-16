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
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                MessageBox.Show(args.ExceptionObject.ToString(), "CVARC Network", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            };
            InternalMain(new CompetitionsSettings());
        }

        public static void DebugMain(CompetitionsSettings networkSettings)
        {
            InternalMain(networkSettings);
        }

        private static void InternalMain(CompetitionsSettings settings)
        {
            try
            {
                InitCompetition(settings);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                form = new TutorialForm(participant.competitionsBundle.competitions);
                new Thread(() => participant.competitionsBundle.competitions.ProcessParticipants(realTime, 1000, participants))
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

        private static void InitCompetition(CompetitionsSettings settings)
        {
            participants = new Participant[2];
            participant = new NetworkParticipant(settings.CompetitionsName);
            participants[participant.ControlledRobot] = participant;
            var botNumber = participant.ControlledRobot == 0 ? 1 : 0;
            participant.competitionsBundle.competitions.Initialize(new CVARCEngine(participant.competitionsBundle.Rules),
                new[] { new RobotSettings(participant.ControlledRobot, false), new RobotSettings(botNumber, true) });
            var botName = participant.HelloPackage.Opponent ?? "None";
            participants[botNumber] = participant.competitionsBundle.competitions.CreateBot(botName, botNumber);
        }
    }
}
