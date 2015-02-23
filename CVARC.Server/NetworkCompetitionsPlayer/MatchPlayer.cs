using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NetworkCompetitionsPlayer.Contracts;

namespace NetworkCompetitionsPlayer
{
    class MatchPlayer
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

        public void Play(MatchResultClient unplayedMatch)
        {
            DisposeResource();
            var taskTimeout = Task.Delay(3000);
            var process = RunCompetition();
            taskTimeout.Wait();
            DisposeResource();
//            unplayedMatch.
        }

        private Process RunCompetition()
        {

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

        private void DisposeResource()
        {
            var proceses = Process.GetProcesses().Where(x => x.MainWindowTitle == "rtsClient");
            foreach (var process in proceses)
            {
                var currentProcess = process;
                SafeAction(currentProcess.Kill);
            }
            SafeAction(() => Directory.Delete(player.Name, true));
            SafeAction(() => Directory.Delete(player2.Name, true));
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
