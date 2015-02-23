using System;
using System.Collections.Generic;
using System.Linq;
using NetworkCompetitionsPlayer.Contracts;

namespace NetworkCompetitionsPlayer
{
    public static class Program
    {
        private static readonly JsonHttpClient Client = new JsonHttpClient();
        private static readonly Dictionary<string, PlayerClient> Players = new Dictionary<string, PlayerClient>();
        private const string LevelName = "Level2";
        
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
                var matchPlayer = new MatchPlayer(LevelName, GetPlayer(unplayedMatch.Player), GetPlayer(unplayedMatch.Player2));
                    matchPlayer.Play(unplayedMatch);
                    //Client.SendRequest(Urls.SaveMatchResult, result);
            }
        }
    }
}
