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
            var competitionsInfo = Client.SendRequest<CompetitionsInfoClient>(Urls.GetCompetitionsInfo);
            var unplayedMatchs = competitionsInfo.MatchResults.Where(x => !x.IsFinished()).ToArray();
            foreach (var unplayedMatch in unplayedMatchs)
            {
                var matchPlayer = new MatchPlayer(Package, GetPlayer(unplayedMatch.Player), GetPlayer(unplayedMatch.Player2));
                    matchPlayer.Play(unplayedMatch);
                    //Client.SendRequest(Urls.SaveMatchResult, result);
            }
        }
    }
}
