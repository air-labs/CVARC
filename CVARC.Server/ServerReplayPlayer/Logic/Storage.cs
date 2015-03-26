using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using CommonTypes;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    static class Storage
    {
        private static readonly string BaseFolder = Helpers.GetServerPath("App_Data");
        private static readonly string TempFolder = Path.Combine(BaseFolder, "Temp");
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
                if (!File.Exists(OpenLevelsFile))
                    ChangeOpenLevels(LevelName.Level1, false);
                var levels = ReadLevelNames();
                openLevels = levels;
            }
            return openLevels.ToArray();
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
            File.WriteAllLines(OpenLevelsFile, currentLevels.Select(x => x.ToString()));
        }

        private static HashSet<LevelName> ReadLevelNames()
        {
            return new HashSet<LevelName>(File.ReadAllLines(OpenLevelsFile).Select(x => (LevelName)Enum.Parse(typeof(LevelName), x)));
        }

        public static void SavePlayerClient(string level, string name, HttpPostedFileBase file)
        {
            var cache = GetCache(level).PlayerCache;
            var exsistingPlayer = cache.TryGetEntity(x => x.Name == name) ?? new PlayerEntity { Id = Guid.NewGuid() };
            exsistingPlayer.Name = name;
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                cache.Save(exsistingPlayer, memoryStream.ToArray());
            }
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
            exsistingResult.Points = matchResult.Points;
            exsistingResult.Player = matchResult.Player.Id;
            if (matchResult.Player2 != null)
                exsistingResult.Player2 = matchResult.Player2.Id;
            cache.Save(exsistingResult, Encoding.UTF8.GetBytes(matchResult.Replay));
        }

        public static string SaveTempFile(HttpPostedFileBase file, string fileName, string folder = null)
        {
            var path = Path.Combine(TempFolder, (folder ?? Guid.NewGuid().ToString()));
            path.CreateDirectoryIfNoExists();
            file.SaveAs(Path.Combine(path, fileName));
            return path;
        }
    }
}