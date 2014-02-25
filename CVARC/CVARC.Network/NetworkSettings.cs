using System.Configuration;

namespace CVARC.Basic
{
    public class NetworkSettings : CompetitionsSettings
    {
        public NetworkSettings()
        {
            IsLocalServer = bool.Parse(ConfigurationManager.AppSettings["IsLocalServer"]);
        }

        public bool IsLocalServer { get; set; }
    }
}