using System.Threading;
using CVARC.Basic;
using CVARC.Network;

namespace ClientBase
{
    public class CvarcClient
    {
        private readonly ClientSettings settings;

        public CvarcClient(string[] args, ClientSettings settings)
        {
            this.settings = settings;
            string relativePath = args.Length == 0 ? "" : args[0];
            StartServer(relativePath);
        }

        public Server<TSensorsData> GetServer<TSensorsData>() where TSensorsData : ISensorsData
        {
            return new Server<TSensorsData>(settings);
        }

        private void StartServer(string path)
        {
            ThreadPool.QueueUserWorkItem(o => Program.DebugMain(new NetworkSettings{CompetitionsName = "Fall2013.0.dll"}));
//            Process.Start(new ProcessStartInfo("CVARC.Network.exe")
//                {
//                    WorkingDirectory = path
//                });
        }
    }
}
