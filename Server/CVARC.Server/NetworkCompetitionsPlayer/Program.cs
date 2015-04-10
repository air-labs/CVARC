using System;
using System.Threading;
using CommonTypes;
using CVARC.Network;
using Timer = System.Timers.Timer;

namespace NetworkCompetitionsPlayer
{
    public static class Program
    {
        private static readonly HelloPackage Package = new HelloPackage
            {
                LevelName = LevelName.Level1,
                MapSeed = 0,
                Opponent = "None"
            };

        private static readonly MatchsPlayer MatchsPlayer = new MatchsPlayer(Package);

        private static void PlayByTimeout(int timeout)
        {
            var t = new Timer(timeout);
            t.Elapsed += (sender, args) => MatchsPlayer.PlayAllMatches();
            t.Start();

            Thread.Sleep(-1);
        }


        [STAThread]
        static void Main()
        {
//            PlayByTimeout(1000 * 60 * 20);
            MatchsPlayer.PlayAllMatches();
        }
    }
}
