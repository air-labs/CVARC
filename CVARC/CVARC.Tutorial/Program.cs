using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Core;
using CVARC.Graphics;
using CVARC.Physics;

namespace CVARC.Tutorial
{
    static class Program
    {
        static Body root;
        static TutorialForm form;
        static Competitions competitions;

        static List<Keys> pressedKeys = new List<Keys>();
        static void MakeCycle(double time, bool realtime)
        {
            double dt=1.0 / 10000;
            for (double t = 0; t < time; t += dt)
            {
                PhysicalManager.MakeIteration(dt, root);
                foreach (Body body in root)
                    body.Update(1 / 60);
            }
            form.BeginInvoke(new Action(form.UpdateScores));
        }

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
                lock (pressedKeys)
                {
                    var commands = pressedKeys.SelectMany(z => competitions.KeyboardController.GetCommand(z)).ToList();
                }



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
           root = competitions.World.Init();
           PhysicalManager.InitializeEngine(PhysicalEngines.Farseer, root);
           var factory = new DrawerFactory(root);
           competitions.Behaviour.InitSensors();
           competitions.Behaviour.Sensors.ForEach(a =>
           {
               foreach (var robot in competitions.World.Robots)
               {
                   var sens = a.GetOne(robot, competitions.World, factory);
                   robot.Sensors.Add(sens);
               }
           });

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new TutorialForm(competitions.World, factory);
            form.KeyDown += form_KeyDown;
            form.KeyUp += form_KeyUp;
         //   Application.Run(new TutorialForm(com., b, kb));
        //    Application.Run(new TestForm());
        }

       
    }
}
