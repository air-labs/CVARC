using System;
using System.Threading;
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

        private static void InitCompetition(CompetitionsSettings settings)
        {
            var participantsServer = new ParticipantsServer(settings.CompetitionsName);
            var participant = participantsServer.GetParticipant();
            competitionsBundle = participantsServer.CompetitionsBundle;
            participants = new Participant[2];
            participants[participant.ControlledRobot] = participant;
            var botNumber = participant.ControlledRobot == 0 ? 1 : 0;
            participantsServer.CompetitionsBundle.competitions.Initialize(new CVARCEngine(participantsServer.CompetitionsBundle.Rules),
                new[] { new RobotSettings(participant.ControlledRobot, false), new RobotSettings(botNumber, true) });
            var botName = participantsServer.CompetitionsBundle.competitions.HelloPackage.Opponent ?? "None";
            participants[botNumber] = participantsServer.CompetitionsBundle.competitions.CreateBot(botName, botNumber);
        }
    }
}
