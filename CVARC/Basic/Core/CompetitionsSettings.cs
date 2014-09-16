using System.Configuration;

namespace CVARC.Basic
{
    public class CompetitionsSettings
    {
        public string CompetitionsName { get; set; }
        public string SettingsFileName { get; private set; }

        public CompetitionsSettings()
        {
            CompetitionsName = ConfigurationManager.AppSettings["CompetitionsName"];
            SettingsFileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName + ".config";
        }
    }
}
