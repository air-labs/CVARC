using System.Diagnostics;
using System.Linq;
using CVARC.Basic;
using System.IO;

namespace ClientBase
{
    public class CvarcClient
    {
        private readonly ClientSettings settings;
		private readonly string networkServerDirectory;
		public CvarcClient(string[] args, ClientSettings settings, string networkServerDirectory = ".\\..\\..\\..\\..\\build\\bin\\NetworkServer")
        {
			this.networkServerDirectory = networkServerDirectory;
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
			var directoryInfo = new DirectoryInfo(networkServerDirectory);
            Process.Start(new ProcessStartInfo("CVARC.Network.exe")
                {
                    WorkingDirectory = directoryInfo.FullName
                });
//            ThreadPool.QueueUserWorkItem(o => Program.InternalMain());
        }
    }
}
