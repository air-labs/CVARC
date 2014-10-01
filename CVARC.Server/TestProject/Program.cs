using CVARC.Basic;
using CVARC.Network;
using RepairTheStarship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestProject
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var Competitions = new Gems.Levels.Level1();
            Competitions.HelloPackage = new HelloPackage { MapSeed = -1 };
            
            var CvarcRules = new GemsRules();
            var CvarcEngine = new CVARCEngine(CvarcRules);

            Competitions.Initialize(
                CvarcEngine,
                new[] { 
                    new RobotSettings(0, true), 
                    new RobotSettings(1, true) 
                });

            var bots = new[] 
            { 
                new SquareMovingBot(), 
                new SquareMovingBot()
            };

            var form = new TutorialForm(Competitions);

            new Thread(() => Competitions.ProcessParticipants(true, int.MaxValue, bots)) 
                { IsBackground = true }
                .Start();

            Application.Run(form);
        }
    }
}
