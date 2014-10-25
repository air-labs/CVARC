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

        public static void RunServerInTheSameThread(string[] args, Action<bool> Control)
        {
            if (args.Length == 0)
            {
                new Action(() =>
                {
                    Thread.Sleep(1000);
                    Control(false);
                }).BeginInvoke(null, null);
                CVARCProgram.Main(new string[] { "Debug", "14000" });
            }
            else
            {
                Control(false);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            var loader = new Loader();

            loader.AddLevel("RepairTheStarship", "Level1", () => new RepairTheStarship.KroR.Level1());
            loader.AddLevel("RepairTheStarship", "Level2", () => new RepairTheStarship.KroR.Level2());
            loader.AddLevel("RepairTheStarship", "Level3", () => new RepairTheStarship.KroR.Level3());
            loader.AddLevel("Demo", "Movement", () => new Demo.KroR.Movement());
            loader.AddLevel("Demo", "Interaction", () => new Demo.KroR.Interaction());
            loader.AddLevel("Demo", "Camera", () => new Demo.KroR.Camera());

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
            var form = new KroRForm(world);
            Application.Run(form);
        }
    }
}
