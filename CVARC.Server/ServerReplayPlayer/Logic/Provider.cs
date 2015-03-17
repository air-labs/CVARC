using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
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

        public CompetitionsInfoServer GetCompetitionsInfo(string level)
        {
            return new CompetitionsInfoServer
            {
                Players = Storage.GetPlayers(level).Select(x => x.Name).ToArray(),
                MatchResults = Storage.GetMatchResults(level)
            };
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

        public ReplaysViewModel[] GetTestReplays()
        {
            var rand = new Random();
            return LevelHelper.GetLevels().Select(x => new ReplaysViewModel
            {
                Level = x.ToString(),
                Replays = Enumerable.Range(1, 11).Select(y => new Summary(Guid.Empty, rand.Next(0, 100) + (x == Level.Level1 ? "" : ":" + rand.Next(0, 100)), "Вася" + y, x == Level.Level1 ? null : "Петя" + y)).ToArray()
            }).ToArray();
        }

        public ReplaysViewModel[] GetReplays()
        {
            return LevelHelper.GetLevels().Select(x => new ReplaysViewModel
            {
                Level = x.ToString(),
                Replays = GetCompetitionsInfo(x.ToString()).MatchResults.Select(y => new Summary(y.Id, y.Points, y.Player, y.Player2)).ToArray()
            }).ToArray();
        }
    }
}
