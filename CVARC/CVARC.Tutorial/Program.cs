using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private static readonly ConcurrentDictionary<Keys, IEnumerable<Command>> PressedKeys = new ConcurrentDictionary<Keys, IEnumerable<Command>>();

        private static void form_KeyUp(object sender, KeyEventArgs e)
        {
            IEnumerable<Command> commands;
            if (PressedKeys.TryRemove(e.KeyCode, out commands))
            {
                if (commands.Any())
                {
                    competitions.ApplyCommand(new Command
                        {
                            Time = 1,
                            RobotId = commands.First().RobotId
                        });
                }
            }
        }
         
        private static void form_KeyDown(object sender, KeyEventArgs e)
        {
            PressedKeys.TryAdd(e.KeyCode, competitions.KeyboardController.GetCommand(e.KeyCode));
        }

        private static void Process()
        {
            while (true)
            {
                var commands = PressedKeys.SelectMany(z => z.Value).ToList();
                Console.WriteLine(commands.Count);
                Debug.WriteLine(commands.Count);
                foreach (var c in commands)
                    competitions.ApplyCommand(c);
                competitions.MakeCycle(0.1, true);
                form.BeginInvoke(new Action(form.UpdateScores));
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                var settings = new TutorialSettings();
                competitions = Competitions.Load(settings.CompetitionsName, "Level1");
                if (settings.HasMap)
                    competitions.World.HelloPackage = new HelloPackage { MapSeed = settings.MapSeed };
                competitions.Initialize();
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
            form.KeyDown += form_KeyDown;
            form.KeyUp += form_KeyUp;
            new Thread(Process) {IsBackground = true}.Start();
            Application.Run(form);
        }
    }
}
