using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Basic.Core.Participants;
using CVARC.Network;

namespace CVARC.Tutorial
{
    internal static class Program
    {
        private const string CompetitionsName = "Fall2013.0.dll";
        private static TutorialForm form;
        private static CompetitionsBundle competitionsBundle;
        private static readonly KeyboardController Controller = new KeyboardController();

        [STAThread]
        static void Main(string[] args)
        {
            competitionsBundle = CompetitionsBundle.Load(CompetitionsName, "Level1");
            competitionsBundle.competitions.HelloPackage = new HelloPackage { MapSeed = 1 };
            competitionsBundle.competitions.Initialize(new CVARCEngine(competitionsBundle.Rules),
                new[] { new RobotSettings(0, false), new RobotSettings(1, true) });

            var botName = args.FirstOrDefault() ?? "None";
            RunForm(competitionsBundle.competitions.CreateBot(botName, 1));
        }

        private static void RunForm(Participant participant)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new TutorialForm(competitionsBundle.competitions) {KeyPreview = true};
            form.KeyDown += (sender, e) => competitionsBundle.competitions.ApplyCommand(Controller.GetCommand(e.KeyCode));
            form.KeyUp += (sender, e) => competitionsBundle.competitions.ApplyCommand(Command.Sleep(0));
            Task.Factory.StartNew(() => competitionsBundle.competitions.ProcessParticipants(true, int.MaxValue, participant));
            Application.Run(form);
        }
    }
}
