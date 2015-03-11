using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    static class Storage
    {
        private static readonly string BaseFolder = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data");
        private static readonly string TempFolder = Path.Combine(BaseFolder, "Temp");
        private static readonly string[] levels = {"1", "2", "3", "4"};
        private static ConcurrentDictionary<string, LevelCaches> levelCaches; 

        static Storage()
        {
            levelCaches = new ConcurrentDictionary<string, LevelCaches>();
            foreach (var level in levels)
            {
                var path = Path.Combine(BaseFolder, level);
                levelCaches.AddOrUpdate(level, x => new LevelCaches(path), (x, y) => new LevelCaches(path));
            }
        }

        public static void SavePlayerClient(string level, string name, HttpPostedFileBase file)
        {
            var cache = levelCaches[level].PlayerCache;
            var exsistingPlayer = cache.TryGetEntity(x => x.Name == name) ?? new Player { Id = Guid.NewGuid() };
            exsistingPlayer.Name = name;
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                cache.Save(exsistingPlayer, memoryStream.ToArray());
            }
        }

        public static byte[] GetPlayerClient(string level, Guid id)
        {
            return levelCaches[level].PlayerCache.GetFile(id);
        }

        public static Player GetPlayer(string level, Guid id)
        {
            return levelCaches[level].PlayerCache.GetEntity(id);
        }

        public static Player[] GetPlayers(string level)
        {
            return levelCaches[level].PlayerCache.GetAllEntities();
        }

        public static MatchResult[] GetMatchResults(string level)
        {
            return levelCaches[level].MatchResultCache.GetAllEntities();
        }

        public static void SaveMatchResult(string level, MatchResultContract matchResult)
        {
            var cache = levelCaches[level].MatchResultCache;
            var exsistingResult = cache.TryGetEntity(x => x.Player == matchResult.Player && x.Player2 == matchResult.Player2) ?? new MatchResult {Id = Guid.NewGuid()};
            exsistingResult.Points = matchResult.Points;
            cache.Save(exsistingResult, Encoding.UTF8.GetBytes(matchResult.Replay));
        }

        public static string SaveTempFile(string fileName, HttpPostedFileBase file)
        {
            var path = Path.Combine(TempFolder, Guid.NewGuid().ToString());
            Directory.CreateDirectory(path);
            file.SaveAs(Path.Combine(path, fileName));
            return path;
        }
    }
}