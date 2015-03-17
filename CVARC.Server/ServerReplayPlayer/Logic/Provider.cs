using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using CommonTypes;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    class Provider
    {
        private const int FiveMbait = 5 * 1024 * 1024;

        public void AddPlayer(string level, HttpPostedFileBase file)
        {
            var name = Path.GetFileNameWithoutExtension(file.FileName);//todo по имени пользователя
            Storage.SavePlayerClient(level, name, file);
        }

        public Player GetPlayer(string level, Guid id)
        {
            return new Player
            {
                Zip = Storage.GetPlayerClient(level, id),
                Name = Storage.GetPlayer(level, id).Name
            };
        }

        private Player ConvertPlayer(PlayerEntity entity)
        {
            return new Player
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public CompetitionsInfo GetCompetitionsInfo(string level)
        {
            var players = Storage.GetPlayers(level).ToDictionary(x => x.Id);
            var results = Storage.GetMatchResults(level).ToDictionary(x => GetKey(x.Player, x.Player2));
            var firstLevel = String.Compare(level, LevelName.Level1.ToString(), StringComparison.OrdinalIgnoreCase) == 0;
            return new CompetitionsInfo
            {
                Level = level,
                MatchResults = (firstLevel ? GetCompetitionsInfoWithoutOpponent(players, results) : GetCompetitionsInfoWithOpponent(players, results)).ToArray()
            };
        }

        private IEnumerable<MatchResult> GetCompetitionsInfoWithOpponent(Dictionary<Guid, PlayerEntity> players, Dictionary<string, MatchResultEntity> results)
        {
            return from player in players.Values
                   from opponent in players.Values
                   where player.Id != opponent.Id
                   select GetMatchResult(players, results, player.Id, opponent.Id);
        }

        private string GetKey(Guid playerId, Guid opponentId)
        {
            return playerId + " " + opponentId;
        }

        private MatchResult GetMatchResult(Dictionary<Guid, PlayerEntity> players, Dictionary<string, MatchResultEntity> results, Guid playerId, Guid opponentId)
        {
            var key = GetKey(playerId, opponentId);
            return new MatchResult
            {
                Id = results.ContainsKey(key) ? results[key].Id : (Guid?)null,
                Points = results.ContainsKey(key) ? results[key].Points : null,
                Player = ConvertPlayer(players[playerId]),
                Player2 = players.ContainsKey(opponentId) ? ConvertPlayer(players[opponentId]) : null
            };
        }

        private IEnumerable<MatchResult> GetCompetitionsInfoWithoutOpponent(Dictionary<Guid, PlayerEntity> players, Dictionary<string, MatchResultEntity> results)
        {
            return players.Values.Select(x => GetMatchResult(players, results, x.Id, Guid.Empty));
        }

        public string GetReplay(string level, Guid id)
        {
            return id == Guid.Empty ? GetTestReplay() : Encoding.UTF8.GetString(Storage.GetReplay(level, id));
        }

        private string GetTestReplay()
        {
            var path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Replays");
            return File.ReadAllLines(Path.Combine(path, "Alexander Ponomarev.Blue")).Last();
        }

        public void SaveMatchResult(string level, MatchResult matchResult)
        {
            Storage.SaveMatchResult(level, matchResult);
        }

        public void SaveInvalidClient(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength < FiveMbait)
                Storage.SaveTempFile(file, file.FileName + " " + Guid.NewGuid(), "invalidClients");
        }

        public CompetitionsInfo[] GetTestCompetitionsInfos()
        {
            var rand = new Random();
            return new[] { LevelName.Level1, LevelName.Level2 }.Select(x => new CompetitionsInfo
            {
                Level = x.ToString(),
                MatchResults = Enumerable.Range(1, 11).Select(y => new MatchResult
                {
                    Id = Guid.Empty,
                    Player = new Player { Name = "Вася" + y },
                    Player2 = new Player { Name = x == LevelName.Level1 ? null : "Петя" + y },
                    Points = rand.Next(0, 100) + (x == LevelName.Level1 ? "" : ":" + rand.Next(0, 100))
                }).ToArray()
            }).ToArray();
        }

        public CompetitionsInfo[] GetCompetitionsInfos()
        {
            return new[] { LevelName.Level1, LevelName.Level2 }.Select(x => GetCompetitionsInfo(x.ToString())).ToArray();
        }
    }
}
