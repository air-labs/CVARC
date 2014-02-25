using System.Configuration;

namespace CVARC.Basic
{
    public abstract class CompetitionsSettings
    {
        public string CompetitionsName { get; private set; }
        public string LevelName { get; private set; }
        public string SettingsFileName { get; private set; }

        public CompetitionsSettings()
        {
            CompetitionsName = ConfigurationManager.AppSettings["CompetitionsName"];
            LevelName = ConfigurationManager.AppSettings["LevelName"];
            SettingsFileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName + ".config";
        }
    }
}
