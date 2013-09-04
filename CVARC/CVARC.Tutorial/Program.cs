using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CVARC.Basic;
using CVARC.Basic.Controllers;

namespace CVARC.Tutorial
{
    static class Program
    {
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
         //   Application.Run(new TutorialForm(com., b, kb));
        //    Application.Run(new TestForm());
        }
    }
}
