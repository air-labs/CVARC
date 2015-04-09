using ServerReplayPlayer.Logic.Storage;

namespace ServerReplayPlayer.Logic.Providers
{
    public class SettingsProvider
    {
        private static readonly string ConfigType = 
            System.Configuration.ConfigurationManager.AppSettings["configType"];

        public static string GetSettingsFilePath(string fileName)
        {
            var folder = ConfigType == "debug" ? "test" : "production";
            return Helpers.GetServerPath(string.Format("settings\\{0}\\{1}", folder, fileName));
        }
    }
}