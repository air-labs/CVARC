using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using CVARC.Basic;
using CVARC.Basic.Core.Participants;
using Gems;

namespace CVARC.Network
{
    public static class Program
    {
        private const string CompetitionsName = "Fall2013.0.dll";
        private static CompetitionsBundle competitionsBundle;

        [STAThread]
        static void Main()
        {
            InternalMain();
        }

        public static void InternalMain()
        {
            SimpleLogger.Run();
            var participants = InitCompetition();
            RunForm(participants);
        }

        private static void RunForm(Participant[] participants)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new TutorialForm(competitionsBundle.competitions);
            Task.Factory.StartNew(() => competitionsBundle.competitions.ProcessParticipants(true, 60 * 1000, participants));
            Application.Run(form);
        }

        private static Participant[] InitCompetition()
        {
            var participantsServer = new ParticipantsServer(CompetitionsName);
            var participant = participantsServer.GetParticipant();
            competitionsBundle = participantsServer.CompetitionsBundle;
            var participants = new Participant[2];
            participants[participant.ControlledRobot] = participant;
            var botNumber = participant.ControlledRobot == 0 ? 1 : 0;
            participantsServer.CompetitionsBundle.competitions.Initialize(new CVARCEngine(participantsServer.CompetitionsBundle.Rules),
                new[] { new RobotSettings(participant.ControlledRobot, false), new RobotSettings(botNumber, true) });
            var botName = participantsServer.CompetitionsBundle.competitions.HelloPackage.Opponent ?? "None";
            participants[botNumber] = participantsServer.CompetitionsBundle.competitions.CreateBot(botName, botNumber);
            return participants;
        }
    }
}
