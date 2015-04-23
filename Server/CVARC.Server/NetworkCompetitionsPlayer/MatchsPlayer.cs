using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using CVARC.Network;
using CommonTypes;

namespace NetworkCompetitionsPlayer
{
    public class MatchsPlayer
    {
        private readonly HelloPackage package;
        private Dictionary<Guid, byte[]> players;
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
                players = new Dictionary<Guid, byte[]>();
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
            try
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
            catch (Exception)
            {
            }
        }

        private PlayerClient GetPlayer(Guid id)
        {
            if (!players.ContainsKey(id))
            {
//                players[id] = GetFromFile(id);
                players[id] = GetFromNetwork(id);
            }
            return new PlayerClient
                {
                    Id = id.ToString(),
                    Zip = players[id]
                };
        }

        private byte[] GetFromFile(Guid id)
        {
            return File.ReadAllBytes(string.Format("C:\\{0}_players\\{1}.file", package.LevelName, id));
        }

        private byte[] GetFromNetwork(Guid id)
        {
           return client.SendRequest<Player>(GetUrl(Urls.GetPlayer, new KeyValuePair<string, string>("id", id.ToString()))).Zip;
        }

        private string GetUrl(string prefix, params KeyValuePair<string, string>[] urlParameters)
        {
            var parameters = urlParameters.Concat(new[] { new KeyValuePair<string, string>("level", package.LevelName.ToString()) });
            return prefix + "?" + string.Join("&", parameters.Select(x => x.Key + "=" + x.Value));
        }
    }
}
