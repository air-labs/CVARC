using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTypes;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic.Providers
{
    public class Provider
    {
        public void AddPlayer(string level, byte[] bytes, string commandName)
        {
            Logger.InfoFormat("Start add Player level={0} command={1}", level, commandName);
            var playerId = Storage.Storage.SavePlayerClient(level, commandName, bytes);
            Logger.InfoFormat("Ok add Player id={0} level={1} command={2}", playerId, level, commandName);
            Storage.Storage.RemoveReplaysByPlayerId(level, playerId);
        }

        public Player GetPlayer(string level, Guid id)
        {
            var player = Storage.Storage.GetPlayer(level, id);
            return new Player
            {
                Id = id,
                CreationDate = player.CreationDate,
                Zip = Storage.Storage.GetPlayerClient(level, id),
                Name = player.Name
            };
        }

        private Player ConvertPlayer(PlayerEntity entity)
        {
            return new Player
            {
                Id = entity.Id,
                Name = entity.Name,
                CreationDate = entity.CreationDate
            };
        }

        private CompetitionsInfo GetCompetitionsInfo(string level)
        {
            var players = Storage.Storage.GetPlayers(level).ToDictionary(x => x.Id);
            var results = Storage.Storage.GetMatchResults(level).ToDictionary(x => GetKey(x.Player, x.Player2));
            var firstLevel = String.Compare(level, LevelName.Level1.ToString(), StringComparison.OrdinalIgnoreCase) == 0;
            var matchResults = (firstLevel ? GetCompetitionsInfoWithoutOpponent(players, results) : GetCompetitionsInfoWithOpponent(players, results));
            return new CompetitionsInfo
            {
                Level = level,
                MatchResults = matchResults.OrderByDescending(x => x.PlayerPoints + x.PlayerPoints)
                                           .ThenByDescending(x => x.IsMatchPlayed)
                                           .ThenBy(x => x.PlayerPoints)
                                           .ToArray()
            };
        }

        private IEnumerable<MatchResult> GetCompetitionsInfoWithOpponent(Dictionary<Guid, PlayerEntity> players, Dictionary<string, MatchResultEntity> results)
        {
            var sortedPlayers = players.Values.OrderBy(x => x.Name).ToArray();
            for (var i = 0; i < sortedPlayers.Length; i++)
            {
                for (var j = i + 1; j < sortedPlayers.Length; j++)
                {
                    var side = (sortedPlayers[i].Name + sortedPlayers[j].Name).GetHashCode() % 2 == 0;
                    var first = side ? sortedPlayers[i] : sortedPlayers[j];
                    var second = side ? sortedPlayers[j] : sortedPlayers[i];
                    yield return GetMatchResult(players, results, first.Id, second.Id);
                }
            }
        }

        private string GetKey(Guid playerId, Guid opponentId)
        {
            return playerId + " " + opponentId;
        }

        private MatchResult GetMatchResult(Dictionary<Guid, PlayerEntity> players, Dictionary<string, MatchResultEntity> results, Guid playerId, Guid opponentId)
        {
            var key = GetKey(playerId, opponentId);
            var result = results.ContainsKey(key) ? results[key] : new MatchResultEntity();
            var points = result.Points == null ? null : result.Points.Split(':');
            return new MatchResult
            {
                Id = result.Id,
                IsMatchPlayed = result.Points != null,
                PlayerPoints = points == null ? 0 : int.Parse(points[0]),
                Player2Points = points == null ? 0 : int.Parse(points[1]),
                Player = ConvertPlayer(players[playerId]),
                Player2 = players.ContainsKey(opponentId) ? ConvertPlayer(players[opponentId]) : null,
            };
        }

        private IEnumerable<MatchResult> GetCompetitionsInfoWithoutOpponent(Dictionary<Guid, PlayerEntity> players, Dictionary<string, MatchResultEntity> results)
        {
            return players.Values.Select(x => GetMatchResult(players, results, x.Id, Guid.Empty));
        }

        public string GetReplay(string level, Guid id)
        {
            return Encoding.UTF8.GetString(Storage.Storage.GetReplay(level, id));
        }

        public void SaveMatchResult(string level, MatchResult matchResult)
        {
            Storage.Storage.SaveMatchResult(level, matchResult);
        }

        public CompetitionsInfo[] GetCompetitionsInfos(string level)
        {
            LevelName levelName;
            var levels = Enum.TryParse(level, out levelName) ? new[] {levelName} : Storage.Storage.GetOpenLevels();
            return levels.Select(x => GetCompetitionsInfo(x.ToString())).ToArray();
        }


        public TeamResult[] GetResult(string[] teams, out string[] levels)
        {
            var info = GetCompetitionsInfos(null);
            levels = info.Select(x => x.Level).ToArray();
            return teams.Select(x => new TeamResult(x,
                info.ToDictionary(y => y.Level, y => 
                    y.MatchResults.Sum(z => z.Player.Name == x ? z.PlayerPoints : 
                        (z.Player2 != null && z.Player2.Name == x ? z.Player2Points : 0))))).ToArray();
        }

        public void ChangeOpenLevel(string level)
        {
            var removeLevel = level.StartsWith("-");
            if (removeLevel)
                level = level.Substring(1);
            Storage.Storage.ChangeOpenLevels((LevelName)Enum.Parse(typeof(LevelName), level), removeLevel);
        }
    }
}
