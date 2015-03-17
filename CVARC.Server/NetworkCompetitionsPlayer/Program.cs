using System;
using System.Collections.Generic;
using System.Linq;
using CommonTypes;
using CVARC.Network;

namespace NetworkCompetitionsPlayer
{
    public static class Program
    {
        private static readonly JsonHttpClient Client = new JsonHttpClient();
        private static readonly Dictionary<string, Player> Players = new Dictionary<string, Player>();
        private static readonly HelloPackage Package = new HelloPackage
            {
                LevelName = LevelName.Level1,
                MapSeed = 1
            };

        private static Player GetPlayer(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            if (!Players.ContainsKey(name))
                Players[name] = Client.SendRequest<Player>(Urls.GetPlayer + "?name=" + name);
            return Players[name];
        }

        [STAThread]
        static void Main()
        {
            var unplayedMatchs = GetUnplayedMatches();
            foreach (var unplayedMatch in unplayedMatchs)
            {
                var matchPlayer = new MatchPlayer(Package, GetPlayer(unplayedMatch.Player), GetPlayer(unplayedMatch.Player2));
                unplayedMatch.Replay = matchPlayer.Play();
//                unplayedMatch.Points = //todo
                Client.SendRequest(Urls.SaveMatchResult, unplayedMatch);
            }
        }

        private static IEnumerable<MatchResult> GetUnplayedMatches()
        {
            var competitionsInfo = Client.SendRequest<CompetitionsInfo>(Urls.GetCompetitionsInfo);
//            if (Package.LevelName == LevelName.Level1)
//                return competitionsInfo.MatchResults.
            var playedMatches = competitionsInfo.MatchResults.ToDictionary(x => x.Player + "_" + x.Player2);
            return from player in competitionsInfo.Players
                   from player2 in competitionsInfo.Players
                   where player != player2
                   where !playedMatches.ContainsKey(player + "_" + player2)
                   select new MatchResult
                       {
                           Player = player,
                           Player2 = player2
                       };
        }
    }
}
