using System.Configuration;
using CVARC.Basic;

namespace CVARC.Network
{
    public class NetworkSettings : CompetitionsSettings
    {
        public NetworkSettings()
        {
            bool startClient;
            bool.TryParse(ConfigurationManager.AppSettings["StartClient"], out startClient);
            StartClient = startClient;
        }

        public bool StartClient { get; private set; }
    }
}