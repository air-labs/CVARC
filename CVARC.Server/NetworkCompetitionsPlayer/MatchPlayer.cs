using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using CVARC.Basic;
using CVARC.Basic.Core.Participants;
using CVARC.Network;
using NetworkCompetitionsPlayer.Contracts;
using CompetitionsSettings = CVARC.Network.CompetitionsSettings;

namespace NetworkCompetitionsPlayer
{
    class MatchPlayer : IDisposable
    {
        private readonly string levelName;
        private readonly PlayerClient player;
        private readonly PlayerClient player2;

        public MatchPlayer(string levelName, PlayerClient player, PlayerClient player2)
        {
            this.levelName = levelName;
            this.player = player;
            this.player2 = player2;
        }

        public MatchResultClient Play()
        {
            CVARC.Network.Program.InternalMain(InitCompetition());
            return new MatchResultClient();
        }

        private CompetitionsSettings InitCompetition()
        {
            var participantsServer = new ParticipantsServer("Fall2013.0.dll");
            var participantsTask = Task.Factory.StartNew(() => participantsServer.GetParticipants(new HelloPackage { LevelName = levelName }));
            RunClients();
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

        private void RunClients()
        {
            UnZipAndRunClient(player);
            Thread.Sleep(100);
            UnZipAndRunClient(player2);
        }

        private void UnZipAndRunClient(PlayerClient playerClient)
        {
            Directory.CreateDirectory(playerClient.Name);
            var zipFilePath = Path.Combine(playerClient.Name, playerClient.Name) + ".zip";
            File.WriteAllBytes(zipFilePath, playerClient.Zip);
            ZipFile.ExtractToDirectory(zipFilePath, playerClient.Name);
            Process.Start(new ProcessStartInfo("run.bat")
                {
                    Arguments = "noRunServer",
                    WorkingDirectory = playerClient.Name
                });
        }

        public void Dispose()
        {
            SafeAction(() => Directory.Delete(player.Name));
            SafeAction(() => Directory.Delete(player2.Name));
        }

        private void SafeAction(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                Logger.Log(e);
            }    
        }
    }
}
