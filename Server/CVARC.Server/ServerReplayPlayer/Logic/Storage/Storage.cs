using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommonTypes;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic.Storage
{
    static class Storage
    {
        private static readonly string BaseFolder = Helpers.GetServerPath("App_Data");
        private static readonly string OpenLevelsFile = Path.Combine(BaseFolder, "openLevels.txt");
        private static readonly ConcurrentDictionary<string, LevelCaches> LevelCaches = new ConcurrentDictionary<string, LevelCaches>();
        private static HashSet<LevelName> openLevels; 

        private static LevelCaches GetCache(string level)
        {
            return LevelCaches.GetOrAdd(level, x => new LevelCaches(Path.Combine(BaseFolder, level)));
        }

        public static LevelName[] GetOpenLevels()
        {
            if (openLevels == null)
            {
                var levels = File.Exists(OpenLevelsFile) ? ReadLevelNames() : CreateOpenLevelsFile();
                openLevels = levels;
            }
            return openLevels.ToArray();
        }

        private static HashSet<LevelName> CreateOpenLevelsFile()
        {
            var levels = new[] {LevelName.Level1};
            WriteLevelNames(levels);
            return new HashSet<LevelName>(levels);
        }

        public static void ChangeOpenLevels(LevelName level, bool remove)
        {
            var currentLevels = ReadLevelNames();
            if (!remove)
            {
                openLevels.Add(level);
                currentLevels.Add(level);                
            }
            else
            {
                openLevels.Remove(level);
                currentLevels.Remove(level);                
            }
            WriteLevelNames(currentLevels);
        }

        private static void WriteLevelNames(IEnumerable<LevelName> levels)
        {
            File.WriteAllLines(OpenLevelsFile, levels.Select(x => x.ToString()));            
        }

        private static HashSet<LevelName> ReadLevelNames()
        {
            return new HashSet<LevelName>(File.ReadAllLines(OpenLevelsFile).Select(x => (LevelName)Enum.Parse(typeof(LevelName), x)));
        }

        public static Guid SavePlayerClient(string level, string name, byte[] bytes)
        {
            var cache = GetCache(level).PlayerCache;
            var exsistingPlayer = cache.TryGetEntity(x => x.Name == name) ?? new PlayerEntity { Id = Guid.NewGuid() };
            exsistingPlayer.Name = name;
            cache.Save(exsistingPlayer, bytes);
            return exsistingPlayer.Id;
        }

        public static byte[] GetPlayerClient(string level, Guid id)
        {
            return GetCache(level).PlayerCache.GetFile(id);
        }

        public static PlayerEntity GetPlayer(string level, Guid id)
        {
            return GetCache(level).PlayerCache.GetEntity(id);
        }

        public static PlayerEntity[] GetPlayers(string level)
        {
            return GetCache(level).PlayerCache.GetAllEntities();
        }

        public static MatchResultEntity[] GetMatchResults(string level)
        {
            return GetCache(level).MatchResultCache.GetAllEntities();
        }

        public static byte[] GetReplay(string level, Guid id)
        {
            return GetCache(level).MatchResultCache.GetFile(id);
        }

        public static void SaveMatchResult(string level, MatchResult matchResult)
        {
            var cache = GetCache(level).MatchResultCache;
            var exsistingResult = cache.TryGetEntity(x => x.Player == matchResult.Player.Id && (matchResult.Player2 == null || x.Player2 == matchResult.Player2.Id)) ?? new MatchResultEntity {Id = Guid.NewGuid()};
            exsistingResult.Points = matchResult.PlayerPoints + ":" + matchResult.Player2Points;
            exsistingResult.Player = matchResult.Player.Id;
            exsistingResult.Player1CreationDate = matchResult.Player.CreationDate;
            if (matchResult.Player2 != null)
            {
                exsistingResult.Player2 = matchResult.Player2.Id;
                exsistingResult.Player2CreationDate = matchResult.Player2.CreationDate;
            }
            cache.Save(exsistingResult, Encoding.UTF8.GetBytes(matchResult.Replay));
        }

        public static void RemoveReplaysByPlayerId(string level, Guid playerId)
        {
            var cache = GetCache(level);
            var replays = cache.MatchResultCache.GetAllEntities();
            foreach (var replay in replays.Where(x => x.Player == playerId || x.Player2 == playerId))
                cache.MatchResultCache.Remove(replay.Id);
        }
    }
}