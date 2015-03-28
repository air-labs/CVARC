﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CommonTypes;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    public class Provider
    {
        public void AddPlayer(string level, HttpPostedFileBase file, string commandName)
        {
            Storage.SavePlayerClient(level, commandName, file);
        }

        public Player GetPlayer(string level, Guid id)
        {
            var player = Storage.GetPlayer(level, id);
            return new Player
            {
                Id = id,
                CreationDate = player.CreationDate,
                Zip = Storage.GetPlayerClient(level, id),
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
            var result = results.ContainsKey(key) ? results[key] : new MatchResultEntity();
            return new MatchResult
            {
                Id = result.Id,
                Points = result.Points,
                Player = ConvertPlayer(players[playerId]),
                Player2 = players.ContainsKey(opponentId) ? ConvertPlayer(players[opponentId]) : null,
                Player1CreationDate = result.Player1CreationDate,
                Player2CreationDate = result.Player2CreationDate
            };
        }

        private IEnumerable<MatchResult> GetCompetitionsInfoWithoutOpponent(Dictionary<Guid, PlayerEntity> players, Dictionary<string, MatchResultEntity> results)
        {
            return players.Values.Select(x => GetMatchResult(players, results, x.Id, Guid.Empty));
        }

        public string GetReplay(string level, Guid id)
        {
            return Encoding.UTF8.GetString(Storage.GetReplay(level, id));
        }

        public void SaveMatchResult(string level, MatchResult matchResult)
        {
            Storage.SaveMatchResult(level, matchResult);
        }

        public void SaveInvalidClient(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength < FileValidator.MaxFileSize)
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

        public CompetitionsInfo[] GetCompetitionsInfos(string level)
        {
            LevelName levelName;
            var levels = Enum.TryParse(level, out levelName) ? new[] {levelName} : Storage.GetOpenLevels();
            return levels.Select(x => GetCompetitionsInfo(x.ToString())).ToArray();
        }

        public void ChangeOpenLevel(string level)
        {
            var removeLevel = level.StartsWith("-");
            if (removeLevel)
                level = level.Substring(1);
            Storage.ChangeOpenLevels((LevelName)Enum.Parse(typeof(LevelName), level), removeLevel);
        }
    }
}