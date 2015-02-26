using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using CVARC.Basic;
using Gems;

namespace CVARC.Network
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SimpleLogger.Run();
            RunForm(SettingsProvider.Get(args));
        }

        private static void RunForm(CompetitionsSettings settings)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new TutorialForm(settings.Competitions);
            Task.Factory.StartNew(() => settings.Competitions.ProcessParticipants(false, 60 * 1000, settings.Participants));
            Application.Run(form);
        }
    }
}
