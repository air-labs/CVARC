using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CVARC.Basic;

namespace CVARC.BotDemo
{
    static class Program
    {

        

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 2 )
            {
                MessageBox.Show("Please specify the assembly with rules, and then bots' names (at least one)", "CVARC BotDemo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var competitions = Competitions.Load(args[0]);
            competitions.Initialize();
            var bot=competitions.CreateBot(args[1],0);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new TutorialForm(competitions);
            new Thread(()=>competitions.ProcessOneParticipant(bot,true)) { IsBackground = true }.Start();
            Application.Run(form);
        }
    }
}
