using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    class Storage
    {
        private static readonly StorageHelper StorageHelper = new StorageHelper();

        public static void AddPlayer(HttpPostedFileBase file)
        {
            var name = Path.GetFileNameWithoutExtension(file.FileName) + ".zip";//todo по имени пользователя
            file.SaveAs(StorageHelper.GetPlayerPath(name));
        }

        public PlayerServer GetPlayer(string name)
        {
            return new PlayerServer
            {
                Zip = File.ReadAllBytes(StorageHelper.GetPlayerPath(name) + ".zip"),
                Name = name
            };
        }

        public CompetitionsInfoServer GetCompetitionsInfo()
        {
            var players = StorageHelper.GetPlayerFiles().Select(Path.GetFileNameWithoutExtension).ToArray();
            return new CompetitionsInfoServer
            {
                Players = players,
                MatchResults = GetAllMatches(players).ToArray()
            };
        }

        private IEnumerable<MatchResultServer> GetAllMatches(string[] players)
        {
            foreach (var player in players)
            {
                foreach (var player2 in players)
                {
                    if (player == player2)
                        continue;
                    var resultPath = StorageHelper.GetMatchResultPath(player, player2);
                    if (File.Exists(resultPath))
                        yield return new MatchResultServer(File.ReadAllLines(resultPath));
                    yield return new MatchResultServer(player, player2);
                }
            }
        }

        public void SaveMatchResult(MatchResultServer matchResult)
        {
            File.WriteAllBytes(StorageHelper.GetReplayPath(matchResult.Player, matchResult.Player2), matchResult.Replay);
            File.WriteAllLines(StorageHelper.GetMatchResultPath(matchResult.Player, matchResult.Player2), new[]
            {
                matchResult.Player,
                matchResult.Player2,
                //todo
            });
        }
    }
}
