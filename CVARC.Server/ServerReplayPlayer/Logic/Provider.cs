using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
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
            return Encoding.UTF8.GetString(Storage.GetReplay(level, id));
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
    }
}
