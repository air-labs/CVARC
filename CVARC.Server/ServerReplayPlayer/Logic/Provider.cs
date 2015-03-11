using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    class Provider
    {
        public static void AddPlayer(HttpPostedFileBase file)
        {
            var name = Path.GetFileNameWithoutExtension(file.FileName);//todo по имени пользователя
            Storage.SavePlayerClient(name, file);
        }

        public PlayerServer GetPlayer(string name)
        {
            return new PlayerServer
            {
                Zip = Storage.ReadPlayerClient(name),
                Name = name
            };
        }

        public CompetitionsInfoServer GetCompetitionsInfo()
        {
            var players = Storage.GetPlayerNames();
            return new CompetitionsInfoServer
            {
                Players = players,
                MatchResults = GetAllMatches().ToArray()
            };
        }

        private IEnumerable<MatchResultServer> GetAllMatches()
        {
            return Storage.GetMatchResults();
        }

        public void SaveMatchResult(MatchResultServer matchResult)
        {
            Storage.SaveMatchResult(matchResult);
        }
    }
}
