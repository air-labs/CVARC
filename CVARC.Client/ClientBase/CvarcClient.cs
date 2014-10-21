using System.Linq;
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
            if (!args.Contains("noRunServer"))
                StartServer();
        }

        public Server<TSensorsData> GetServer<TSensorsData>() where TSensorsData : ISensorsData
        {
            return new Server<TSensorsData>(settings);
        }

        private void StartServer()
        {
            ThreadPool.QueueUserWorkItem(o => Program.DebugMain(new CompetitionsSettings{CompetitionsName = "Fall2013.0.dll"}));
        }
    }
}
