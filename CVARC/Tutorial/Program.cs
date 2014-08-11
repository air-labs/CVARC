using System;
using System.Threading;
using System.Windows.Forms;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Network;

namespace CVARC.Tutorial
{
    internal static class Program
    {
        private static TutorialForm form;
        private static Competitions competitions;
        private static readonly KeyboardController Controller = new KeyboardController();
        private const string BotName = "Sanguine";

        [STAThread]
        private static void Main()
        {
            try
            {
                var settings = new TutorialSettings();
                competitions = Competitions.Load(settings.CompetitionsName, "Level1");
                if (settings.HasMap)
                    competitions.HelloPackage = new HelloPackage { MapSeed = settings.MapSeed };

                competitions.Initialize(new CVARCEngine(competitions.CvarcRules));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "CVARC Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new TutorialForm(competitions);
            form.KeyPreview = true;
            form.KeyDown += (sender, e) => competitions.ApplyCommand(Controller.GetCommand(e.KeyCode));
            form.KeyUp += (sender, e) => competitions.ApplyCommand(Command.Sleep(0));
            new Thread(() => competitions.ProcessParticipants(true, int.MaxValue, new[] { competitions.CreateBot(BotName, 1) }))
                {
                    IsBackground = true
                }.Start();
            Application.Run(form);
        }
    }
}
