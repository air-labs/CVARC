using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Core;
using CVARC.Graphics;
using CVARC.Network;
using CVARC.Physics;

namespace CVARC.Tutorial
{
    static class Program
    {
        static TutorialForm form;
        static Competitions competitions;

        static List<Keys> pressedKeys = new List<Keys>();
        

        static void form_KeyUp(object sender, KeyEventArgs e)
        {
            lock (pressedKeys)
            {
                pressedKeys.Remove(e.KeyCode);
            }
        }

        static void form_KeyDown(object sender, KeyEventArgs e)
        {
            lock (pressedKeys)
            {
                if (!pressedKeys.Contains(e.KeyCode)) pressedKeys.Add(e.KeyCode);
            }
        }

        static void Process()
        {
            while (true)
            {
                List<Command> commands=null;
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
        static void Main(string[] args)
       {
           if (args.Length == 0)
           {
               MessageBox.Show("Please specify the assembly with rules", "CVARC Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Error);
               return;
           }

           competitions = Competitions.Load(args[0]);
            if (args.Length > 2)
            {
                if (args[1] == "--map")
                {
                    int map;
                    if (!int.TryParse(args[2], out map))
                        map = -1;
                    competitions.World.HelloPackage = new HelloPackage {RandomMapSeed = map};
                }
            }
           competitions.Initialize();
           

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new TutorialForm(competitions);
            form.KeyPreview = true;
            form.KeyDown += form_KeyDown;
            form.KeyUp += form_KeyUp;
            new Thread(Process) { IsBackground = true }.Start();
            Application.Run(form);
        }
    }
}
