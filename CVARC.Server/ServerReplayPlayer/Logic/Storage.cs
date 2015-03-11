using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Hosting;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    static class Storage
    {
        private static readonly string BaseFolder = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data");
        private static readonly string TempFolder = Path.Combine(BaseFolder, "temp");
        private static readonly string PlayerFolder = Path.Combine(BaseFolder, "players_level1");
        private static readonly string ReplayFolder = Path.Combine(BaseFolder, "replays_level1");
        private static ConcurrentDictionary<string, Player> players;
        private static ConcurrentDictionary<string, Player> Players
        {
            get
            {
                return players ?? (players = new ConcurrentDictionary<string, Player>(Directory
                    .GetFiles(PlayerFolder)
                    .Where(x => !x.EndsWith(".zip"))
                    .Select(x =>
                        {
                            var player = ReadFile<Player>(x);
                            return new KeyValuePair<string, Player>(player.Name, player);
                        })));
            }
        }

        static Storage()
        {
            CreateDirectoryIfNotExsists(PlayerFolder);
            CreateDirectoryIfNotExsists(ReplayFolder);
            CreateDirectoryIfNotExsists(TempFolder);
        }

        private static void CreateDirectoryIfNotExsists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static void SavePlayerClient(string name, HttpPostedFileBase file)
        {
            bool isNew = false;
            var id = Players.GetOrAdd(name, s =>
                {
                    var player = new Player {Name = name};
                    SaveFile(player, PlayerFolder);
                    isNew = true;
                    return player;
                }).Id;
            var path = Path.Combine(PlayerFolder, id + ".zip");
            if (!isNew)
                File.Delete(path);
            file.SaveAs(path);
        }

        public static byte[] ReadPlayerClient(string name)
        {
            var path = Path.Combine(PlayerFolder, Players[name].Id + ".zip");
            return File.ReadAllBytes(path);
        }

        public static string[] GetPlayerNames()
        {
            return Players.Select(x => x.Key).ToArray();
        }

        public static MatchResultServer[] GetMatchResults(bool needReplay = false)
        {
            return Directory.GetFiles(ReplayFolder).Select(x =>
                {
                    var matchResult = ReadFile<MatchResultServer>(x);
                    if (!needReplay)
                        matchResult.Replay = null;
                    return matchResult;
                }).ToArray();
        }

        public static string SaveTempFile(string fileName, HttpPostedFileBase file)
        {
            var path = Path.Combine(TempFolder, Guid.NewGuid().ToString());
            Directory.CreateDirectory(path);
            file.SaveAs(Path.Combine(path, fileName));
            return path;
        }

        public static void SaveMatchResult(MatchResultServer result)
        {
            SaveFile(result, ReplayFolder);
        }

        private static void SaveFile<T>(T data, string path) where T : IWithId
        {
            data.Id = Guid.NewGuid();
            using (var file = File.Create(Path.Combine(path, data.Id.ToString())))
                new BinaryFormatter().Serialize(file, data);
        }

        private static T ReadFile<T>(string path)
        {
            using (var file = File.Open(path, FileMode.Open))
                return (T)new BinaryFormatter().Deserialize(file);
        }
    }
}