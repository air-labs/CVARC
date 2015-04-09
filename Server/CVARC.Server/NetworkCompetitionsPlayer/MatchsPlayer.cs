using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CVARC.Network;
using CommonTypes;

namespace NetworkCompetitionsPlayer
{
    public class MatchsPlayer
    {
        private readonly HelloPackage package;
        private Dictionary<Guid, Player> players;
        private readonly JsonHttpClient client = new JsonHttpClient();
        private readonly AutoResetEvent autoResetEvent = new AutoResetEvent(true);

        public MatchsPlayer(HelloPackage package)
        {
            this.package = package;
        }

        public void PlayAllMatches()
        {
            if (!autoResetEvent.WaitOne(0))
                return;
            try
            {
                players = new Dictionary<Guid, Player>();
                var unplayedMatchs = GetUnplyedMatches();
                foreach (var unplayedMatch in unplayedMatchs)
                    PlayMatch(unplayedMatch);
            }
            catch (Exception e)
            {
                Logger.Log(e);
            }
            finally
            {
                autoResetEvent.Set();                
            }
        }

        private MatchResult[] GetUnplyedMatches()
        {
            return client.SendRequest<CompetitionsInfo>(GetUrl(Urls.GetCompetitionsInfo))
                                          .MatchResults.Where(x => !x.IsMatchPlayed)
                                          .ToArray();
        }

        private void PlayMatch(MatchResult unplayedMatch)
        {
            var player1 = GetPlayer(unplayedMatch.Player.Id);
            var player2 = unplayedMatch.Player2 == null ? null : GetPlayer(unplayedMatch.Player2.Id);
            var matchPlayer = new MatchPlayer(package, player1, player2);
            var replayFile = matchPlayer.Play();
            var splits = replayFile.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            unplayedMatch.Replay = splits.Last();
            var points = splits[1].Split(':');
            unplayedMatch.PlayerPoints = int.Parse(points[0]);
            unplayedMatch.Player2Points = int.Parse(points[1]);
            client.SendRequest(GetUrl(Urls.SaveMatchResult), unplayedMatch);
        }

        private Player GetPlayer(Guid id)
        {
            if (!players.ContainsKey(id))
                players[id] = client.SendRequest<Player>(GetUrl(Urls.GetPlayer, new KeyValuePair<string, string>("id", id.ToString())));
            return players[id];
        }

        private string GetUrl(string prefix, params KeyValuePair<string, string>[] urlParameters)
        {
            var parameters = urlParameters.Concat(new[] { new KeyValuePair<string, string>("level", package.LevelName.ToString()) });
            return prefix + "?" + string.Join("&", parameters.Select(x => x.Key + "=" + x.Value));
        }
    }
}
