using System;
using System.Collections.Generic;
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
            competitions.ProcessOneParticipant(true, participant);
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
            var parts = new List<IParticipant>();
            
            participant = new NetworkParticipant(competitions);
            parts.Add(participant);

            if (participant.HelloPackage.Opponent != null)
            {
                var botNumber = participant.HelloPackage.LeftSide ? 1 : 0;
                parts.Add(competitions.CreateBot(participant.HelloPackage.Opponent, botNumber));
            }

            competitions.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new TutorialForm(competitions);

            new Thread(()=>competitions.ProcessParticipants(true,parts.ToArray())) { IsBackground = true }.Start();

            Application.Run(form);
            
        }
    }
}
