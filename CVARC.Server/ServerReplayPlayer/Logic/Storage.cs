using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using CommonTypes;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    static class Storage
    {
        private static readonly string BaseFolder = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data");
        private static readonly string TempFolder = Path.Combine(BaseFolder, "Temp");
        private static readonly ConcurrentDictionary<string, LevelCaches> LevelCaches = new ConcurrentDictionary<string, LevelCaches>(); 

        private static LevelCaches GetCache(string level)
        {
            return LevelCaches.GetOrAdd(level, x => new LevelCaches(Path.Combine(BaseFolder, level)));
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
            var exsistingResult = cache.TryGetEntity(x => x.Player == matchResult.Player && x.Player2 == matchResult.Player2) ?? new MatchResultEntity {Id = Guid.NewGuid()};
            exsistingResult.Points = matchResult.Points;
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