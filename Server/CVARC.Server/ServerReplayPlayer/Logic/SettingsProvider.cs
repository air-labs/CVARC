namespace ServerReplayPlayer.Logic
{
    public class SettingsProvider
    {
        private static readonly string ConfigType = 
            System.Configuration.ConfigurationManager.AppSettings["configType"];

        public static string GetSettingsFilePath(string fileName)
        {
            return Helpers.GetServerPath(string.Format("settings\\{0}\\{1}", ConfigType, fileName));
        }
    }
}