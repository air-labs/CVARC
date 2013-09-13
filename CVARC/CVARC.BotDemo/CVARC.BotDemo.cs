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
            List<Bot> bots = new List<Bot>();
            for (int i=0;i<competitions.World.RobotCount;i++)
            {
                if (i+1>=args.Length) break;
                var botName=args[i+1];
                if (botName=="None") continue;
                bots.Add(competitions.CreateBot(args[i + 1], i));
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new TutorialForm(competitions);
            new Thread(() => competitions.ProcessParticipants(true, int.MaxValue,  bots.ToArray())) { IsBackground = true }.Start();
            Application.Run(form);
        }
    }
}
