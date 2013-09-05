using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Network;

namespace CVARK.Network
{


    static class Program
    {

        static Competitions competitions;
        static NetworkParticipant participant;
        static Form form;



        static void Process()
        {
            competitions.ProcessOneParticipant(participant, true);
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

            participant = new NetworkParticipant(competitions);

            competitions.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new TutorialForm(competitions);

            new Thread(Process) { IsBackground = true }.Start();

            Application.Run(form);
            
        }
    }
}
