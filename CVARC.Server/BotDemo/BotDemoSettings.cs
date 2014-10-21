using System.Configuration;
using System.Linq;
using CVARC.Basic;

namespace CVARC.BotDemo
{
    class BotDemoSettings : CompetitionsSettings
    {
        public string[] BotNames { get; set; }

        public BotDemoSettings()
        {
            BotNames = ConfigurationManager.AppSettings["BotNames"].Split(';').ToArray();
        }
    }
}
