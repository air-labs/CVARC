using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CVARC.Basic;
using CVARC.Basic.Controllers;

namespace CVARK.Network
{
    static class Program
    {

        static void Process()
        {


        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                MessageBox.Show("Please specify the assembly with rules", "CVARC Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var competitions = Competitions.Load(args[0]);
            

            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += (sender, eventArgs) => Application.Exit();
            var frm = new TutorialForm(w, b, kb, port) {Opacity = 0, ShowInTaskbar = false};
            frm.Closed += (sender, eventArgs) => Environment.Exit(0);
            Application.Run(frm);
        }
    }
}
