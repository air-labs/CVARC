using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using CVARC.Basic;
using CVARC.Network;

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
                CompetitionsBundle.competitions.HelloPackage = new HelloPackage { MapSeed = -1 };
                CompetitionsBundle.competitions.Initialize(new CVARCEngine(CompetitionsBundle.Rules), new[] { new RobotSettings(0, true), new RobotSettings(0, true) });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "CVARC BotDemo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<Bot> bots = new List<Bot>();
            for (int i = 0; i < CompetitionsBundle.competitions.RobotCount; i++)
            {
                if (i == settings.BotNames.Length) break;
                if (settings.BotNames[i] == "None") continue;
                bots.Add(CompetitionsBundle.competitions.CreateBot(settings.BotNames[i], i));
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new TutorialForm(CompetitionsBundle.competitions);
            new Thread(() => CompetitionsBundle.competitions.ProcessParticipants(true, int.MaxValue, bots.ToArray())) { IsBackground = true }.Start();
            Application.Run(form);
        }
    }
}
