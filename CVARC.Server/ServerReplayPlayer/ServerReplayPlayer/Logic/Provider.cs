using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    class Provider
    {
        private static readonly Storage Storage = new Storage();

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
                MatchResults = GetAllMatches(players).ToArray()
            };
        }

        private IEnumerable<MatchResultServer> GetAllMatches(string[] players)
        {
            return from player in players 
                   from player2 in players 
                   where player != player2 
                   select Storage.GetMatchResult(player, player2);
        }

        public void SaveMatchResult(MatchResultServer matchResult)
        {
            Storage.SaveMatchResult(matchResult);
        }
    }
}
