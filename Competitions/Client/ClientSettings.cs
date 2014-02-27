using System;
using System.Configuration;

namespace Client
{
    public class ClientSettings
    {
        public ClientSettings()
        {
            Mode m;
            var mode = ConfigurationManager.AppSettings["Mode"];
            var port = ConfigurationManager.AppSettings["Port"];
            var ip = ConfigurationManager.AppSettings["Ip"];
            Enum.TryParse(mode, out m);
            Mode = m;
            Ip = m == Mode.RealServer ? ip : LocalIp;
            Port = m == Mode.RealServer ? int.Parse(port): LocalPort;
        }

        public Mode Mode { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        private const string LocalIp = "127.0.0.1";
        private const int LocalPort = 14000;
    }

    public enum Mode
    {
        Tutorial,
        LocalServer,
        RealServer
    }
}