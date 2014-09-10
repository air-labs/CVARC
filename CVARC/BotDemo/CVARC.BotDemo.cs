using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using CVARC.Basic;
using CVARC.Network;
using CVARC.Basic.Core;

namespace CVARC.BotDemo
{
    static class Program
    {
        private static CompetitionsBundle CompetitionsBundle;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var settings = new BotDemoSettings();
            try
            {
                CompetitionsBundle = CompetitionsBundle.Load(settings.CompetitionsName, "Level1");
                CompetitionsBundle.Competitions.HelloPackage = new HelloPackage { MapSeed = -1 };
                CompetitionsBundle.Competitions.Initialize(new CVARCEngine(CompetitionsBundle.CvarcRules));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "CVARC BotDemo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<Bot> bots = new List<Bot>();
            for (int i = 0; i < CompetitionsBundle.Competitions.RobotCount; i++)
            {
                if (i == settings.BotNames.Length) break;
                if (settings.BotNames[i] == "None") continue;
                bots.Add(CompetitionsBundle.Competitions.CreateBot(settings.BotNames[i], i));
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new TutorialForm(CompetitionsBundle.Competitions);
            new Thread(() => CompetitionsBundle.Competitions.ProcessParticipants(true, int.MaxValue, bots.ToArray())) { IsBackground = true }.Start();
            Application.Run(form);
        }
    }
}
