using System.Diagnostics;
using System.Linq;
using CVARC.Basic;

namespace ClientBase
{
    public class CvarcClient
    {
        private readonly ClientSettings settings;
		public CvarcClient(string[] args, ClientSettings settings)
        {
            this.settings = settings;
            if (!args.Contains("noRunServer"))
                StartServer();
        }

        public Server<TSensorsData> GetServer<TSensorsData>() where TSensorsData : ISensorsData
        {
            return new Server<TSensorsData>(settings);
        }

        private void StartServer()
        {
            Process.Start(new ProcessStartInfo("CVARC.Network.exe"));
//            ThreadPool.QueueUserWorkItem(o => Program.Main(new string[0]));
        }
    }
}
