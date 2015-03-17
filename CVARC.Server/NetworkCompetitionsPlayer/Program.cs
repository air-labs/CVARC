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
        private static readonly Dictionary<Guid, Player> Players = new Dictionary<Guid, Player>();
        private static readonly HelloPackage Package = new HelloPackage
            {
                LevelName = LevelName.Level1,
                MapSeed = 1,
                Opponent = "None"
            };

        private static Player GetPlayer(Guid id)
        {
            if (!Players.ContainsKey(id))
                Players[id] = Client.SendRequest<Player>(GetUrl(Urls.GetPlayer, new KeyValuePair<string, string>("id", id.ToString())));
            return Players[id];
        }

        [STAThread]
        static void Main()
        {
            var unplayedMatchs = Client.SendRequest<CompetitionsInfo>(GetUrl(Urls.GetCompetitionsInfo)).MatchResults.Where(x => string.IsNullOrEmpty(x.Points)).ToArray();
            foreach (var unplayedMatch in unplayedMatchs)
            {
                var matchPlayer = new MatchPlayer(Package, GetPlayer(unplayedMatch.Player.Id), unplayedMatch.Player2 == null ? null : GetPlayer(unplayedMatch.Player2.Id));
                unplayedMatch.Replay = matchPlayer.Play();
//                unplayedMatch.Points = //todo
                Client.SendRequest(GetUrl(Urls.SaveMatchResult), unplayedMatch);
            }
        }

        private static string GetUrl(string prefix, params KeyValuePair<string, string>[] urlParameters)
        {
            var parameters = urlParameters.Concat(new[] {new KeyValuePair<string, string>("level", Package.LevelName.ToString())}); 
            return prefix + "?" + string.Join("&", parameters.Select(x => x.Key + "=" + x.Value));
        }
    }
}
