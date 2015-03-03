using System;
using System.Collections.Generic;
using System.Linq;
using CVARC.Network;
using NetworkCompetitionsPlayer.Contracts;

namespace NetworkCompetitionsPlayer
{
    public static class Program
    {
        private static readonly JsonHttpClient Client = new JsonHttpClient();
        private static readonly Dictionary<string, PlayerClient> Players = new Dictionary<string, PlayerClient>();
        private static readonly HelloPackage Package = new HelloPackage
            {
                LevelName = "Level2",
                MapSeed = 1
            };
        
        private static PlayerClient GetPlayer(string name)
        {
            if (!Players.ContainsKey(name))
                Players[name] = Client.SendRequest<PlayerClient>(Urls.GetPlayer + "?name=" + name);
            return Players[name];
        }

        [STAThread]
        static void Main()
        {
            var unplayedMatchs = GetUnplayedMatches();
            foreach (var unplayedMatch in unplayedMatchs)
            {
                var matchPlayer = new MatchPlayer(Package, GetPlayer(unplayedMatch.Player), GetPlayer(unplayedMatch.Player2));
                unplayedMatch.Replay =  matchPlayer.Play();
                Client.SendRequest(Urls.SaveMatchResult, unplayedMatch);
            }
        }

        private static IEnumerable<MatchResultClient> GetUnplayedMatches()
        {
            var competitionsInfo = Client.SendRequest<CompetitionsInfoClient>(Urls.GetCompetitionsInfo);
            var playedMatches = competitionsInfo.MatchResults.ToDictionary(x => x.Player + "_" + x.Player2);
            return from player in competitionsInfo.Players
                   from player2 in competitionsInfo.Players
                   where player != player2
                   where !playedMatches.ContainsKey(player + "_" + player2)
                   select new MatchResultClient
                       {
                           Player = player,
                           Player2 = player2
                       };
        }
    }
}
