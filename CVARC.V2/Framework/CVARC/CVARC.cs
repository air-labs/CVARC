using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using CVARC.V2;

namespace CVARC.V2
{
    public static class CVARCProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                MessageBox.Show(Loader.Help, "CVARC", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var loader = new Loader();

            loader.AddLevel("RepairTheStarship", "Level1", () => new RepairTheStarship.KroR.Level1());
            loader.AddLevel("Demo", "Level1", () => new DemoCompetitions.KroR.Level1());

            IWorld world;
            try
            {
                world = loader.Load(args);
            }
            catch (Exception e)
            {
                MessageBox.Show("Initialization failed. " + e.Message, "CVARC", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var form = new KroRForm(world);
            Application.Run(form);
        }
    }
}
