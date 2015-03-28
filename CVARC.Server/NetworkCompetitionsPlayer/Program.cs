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
        private static readonly HelloPackage Package = new HelloPackage
            {
                LevelName = LevelName.Level3,
                MapSeed = 0,
                Opponent = "None"
            };

        private static Player GetPlayer(Guid id)
        {
            return Client.SendRequest<Player>(GetUrl(Urls.GetPlayer, new KeyValuePair<string, string>("id", id.ToString())));
        }

        [STAThread]
        static void Main()
        {
            var macthes = Client.SendRequest<CompetitionsInfo>(GetUrl(Urls.GetCompetitionsInfo)).MatchResults;
            var players = macthes.Select(x => x.Player.Id).Distinct().Select(GetPlayer).ToDictionary(x => x.Id);
            var unplayedMatchs = macthes
                .Where(
                    x => string.IsNullOrEmpty(x.Points)
                         || x.Player1CreationDate < players[x.Player.Id].CreationDate
                         || (x.Player2 != null && x.Player2CreationDate < players[x.Player2.Id].CreationDate))
                .ToArray();
            foreach (var unplayedMatch in unplayedMatchs)
            {
                var matchPlayer = new MatchPlayer(Package, players[unplayedMatch.Player.Id], unplayedMatch.Player2 == null ? null : players[unplayedMatch.Player2.Id]);
                var replayFile = matchPlayer.Play();
                var splits = replayFile.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                unplayedMatch.Replay = splits.Last();
                unplayedMatch.Points = splits[1];
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
