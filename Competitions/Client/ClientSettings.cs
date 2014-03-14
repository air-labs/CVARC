using System.Configuration;
using CVARC.Basic.Core;

namespace Client
{
    public class ClientSettings
    {
        public ClientSettings()
        {
            Mode = ConfigurationManager.AppSettings["Mode"].ParseEnum<Mode>();
            Ip = Mode == Mode.RealServer ? ConfigurationManager.AppSettings["Ip"] : LocalIp;
            Port = Mode == Mode.RealServer ? int.Parse(ConfigurationManager.AppSettings["Port"]): LocalPort;
            BotName = ConfigurationManager.AppSettings["BotName"] ?? DefaultBotName;
            MapSeed = ConfigurationManager.AppSettings["MapSeed"].SafeParseInt();
        }

        public Mode Mode { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public int MapSeed { get; set; }
        public string BotName { get; set; }

        private const string LocalIp = "127.0.0.1";
        private const int LocalPort = 14000;
        private const string DefaultBotName = "None";
    }

    public enum Mode
    {
        Tutorial,
        LocalServer,
        RealServer
    }
}