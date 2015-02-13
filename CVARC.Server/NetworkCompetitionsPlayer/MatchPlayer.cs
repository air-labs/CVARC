using System.Threading.Tasks;
using CVARC.Basic;
using CVARC.Basic.Core.Participants;
using CVARC.Network;
using NetworkCompetitionsPlayer.Contracts;
using CompetitionsSettings = CVARC.Network.CompetitionsSettings;

namespace NetworkCompetitionsPlayer
{
    class MatchPlayer
    {
        public MatchResultClient Play(string levelName, PlayerClient player, PlayerClient player2)
        {
            CVARC.Network.Program.InternalMain(InitCompetition(levelName, player, player2));
            return new MatchResultClient();
        }

        private static CompetitionsSettings InitCompetition(string levelname, PlayerClient player, PlayerClient player2)
        {
            var participantsServer = new ParticipantsServer("Fall2013.0.dll");
            var participantsTask = Task.Factory.StartNew(() => participantsServer.GetParticipants(new HelloPackage {LevelName = levelname}));
            RunClients(player, player2);
            var participants = participantsTask.Result;
            participantsServer.CompetitionsBundle.competitions.Initialize(new CVARCEngine(participantsServer.CompetitionsBundle.Rules), new[]
            {
                new RobotSettings(0, false), 
                new RobotSettings(1, false)
            });
            return new CompetitionsSettings
            {
                Participants = participants,
                RealTime = false,
                CompetitionsBundle = participantsServer.CompetitionsBundle
            };
        }

        private static void RunClients(PlayerClient player, PlayerClient player2)
        {
//            Process.Start("..\\..\\participants\\" + firstPlayer, "noRunServer");
//            Thread.Sleep(100);
//            Process.Start("..\\..\\participants\\" + secondPlayer, "noRunServer");
        }
    }
}
