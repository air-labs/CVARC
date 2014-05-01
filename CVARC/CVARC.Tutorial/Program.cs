using System;
using System.Collections.Generic;
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
        private static List<Keys> pressedKeys = new List<Keys>();

        private static void form_KeyUp(object sender, KeyEventArgs e)
        {
            lock (pressedKeys)
            {
                pressedKeys.Remove(e.KeyCode);
            }
        }
         
        private static void form_KeyDown(object sender, KeyEventArgs e)
        {
            lock (pressedKeys)
            {
                if (!pressedKeys.Contains(e.KeyCode)) pressedKeys.Add(e.KeyCode);
            }
        }

        private static void Process()
        {
            while (true)
            {
                List<Command> commands = null;
                lock (pressedKeys)
                {
                    commands = pressedKeys.SelectMany(z => competitions.KeyboardController.GetCommand(z)).ToList();
                }
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
