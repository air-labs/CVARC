using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace ServerReplayPlayer.Logic
{
    public class StorageHelper
    {
        private readonly string baseFolder = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data");

        private string GetPath(string folder, string fileName)
        {
            return Path.Combine(baseFolder, Path.Combine(folder, fileName));
        }

        private string GetPlayersPath(string player, string player2)
        {
            return string.Format("{0}vs{1}", player, player2);
        }

        public string GetPlayerPath(string name)
        {
            return GetPath("players", name);
        }

        public string GetMatchResultPath(string player, string player2)
        {
            return GetPath("results", GetPlayersPath(player, player2));
        }

        public string GetReplayPath(string player, string player2)
        {
            return GetPath("replays", GetPlayersPath(player, player2));
        }

        public string[] GetPlayerFiles()
        {
            return Directory.GetFiles(Path.Combine(baseFolder, "players"));
        }
    }
}
