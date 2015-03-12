using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CVARC.V2;

namespace CVARC.V2
{
    public static class CVARCProgram
    {

        public static void RunServerInTheSameThread(Action<int> Control)
        {

            var nsData = new NetworkServerData();
            nsData.Port = Loader.DefaultPort;

            new Action(() =>
            {
                nsData.WaitForServer();
                Control(nsData.Port);
            }).BeginInvoke(null, null);

            var loader = GetLoader();
            loader.CreateSoloNetworkWithData(nsData);
            RunWorld(nsData.World);
        }

        public static void RunWorld(IWorld world)
        {
            var form = new KroRForm(world);
            Application.Run(form);

        }

        public static Loader GetLoader()
        {
            var loader = new Loader();

			//loader.AddLevel("RepairTheStarship", "Level1", () => new RepairTheStarship.KroR.Level1());
			//loader.AddLevel("RepairTheStarship", "Level2", () => new RepairTheStarship.KroR.Level2());
			//loader.AddLevel("RepairTheStarship", "Level3", () => new RepairTheStarship.KroR.Level3());
			loader.AddLevel("Demo", "Demo", () => new Demo.KroR.DemoCompetitions());
      
            return loader;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            var loader = GetLoader();
             IWorld world;

            if (false)
            {
                try
                {
                    world = loader.Load(args);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Initialization failed. " + e.Message, "CVARC", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
                world = loader.Load(args);
            RunWorld(world);
        }
    }
}
