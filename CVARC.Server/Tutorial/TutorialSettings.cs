using System.Configuration;

namespace CVARC.Basic
{
    public class TutorialSettings : CompetitionsSettings
    {
        public TutorialSettings()
        {
            HasMap = int.TryParse(ConfigurationManager.AppSettings["Map"], out MapSeed);
        }

        public readonly int MapSeed;
        public bool HasMap { get;private set; }
    }
}