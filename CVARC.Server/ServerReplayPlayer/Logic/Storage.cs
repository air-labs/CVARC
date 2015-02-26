using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    class Storage
    {
        private static readonly StorageHelper StorageHelper = new StorageHelper();
        
        public void SavePlayerClient(string name, HttpPostedFileBase file)
        {
            file.SaveAs(StorageHelper.GetPlayerPath(name) + ".zip");
        }

        public byte[] ReadPlayerClient(string name)
        {
            return File.ReadAllBytes(StorageHelper.GetPlayerPath(name) + ".zip");
        }

        public string[] GetPlayerNames()
        {
            return StorageHelper.GetPlayerFiles().Select(Path.GetFileNameWithoutExtension).ToArray();
        }

        public MatchResultServer GetMatchResult(string player, string player2)
        {
            var resultPath = StorageHelper.GetMatchResultPath(player, player2);
            if (File.Exists(resultPath))
            {
                var file = File.Open(resultPath, FileMode.Open);
                return (MatchResultServer)new BinaryFormatter().Deserialize(file);
            }
            return new MatchResultServer(player, player2);
        }

        public void SaveMatchResult(MatchResultServer result)
        {
            var resultPath = StorageHelper.GetMatchResultPath(result.Player, result.Player2);
            var file = File.Create(resultPath);
            new BinaryFormatter().Serialize(file, result);
        }
    }
}