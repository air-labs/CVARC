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
        static IParticipant[] participants;
        static Form form;
        static Thread processThread;
        static bool goodByeFlag = false;

        static void Process()
        {
            competitions.ProcessParticipants(false, participants);
            GoodBye();
        }

        static void TimeLimit()
        {
            Thread.Sleep((int)(competitions.NetworkTimeLimit * 1000));
            if (processThread.IsAlive)
            {
                processThread.Abort();
            }
            GoodBye();
            
        }

        static void GoodBye()
        {
            if (goodByeFlag) return;
            goodByeFlag = true;
            var replayId=competitions.SendPostReplay(participant.HelloPackage.AccessKey, 0, participant.ControlledRobot);
            participant.SendReplay(replayId);
            Application.Exit();
        }

        static void SendError(Exception exception)
        {
            try
            {
                if (participant == null) return;
                if (exception is UserInputException)
                    participant.SendError(exception.InnerException, true);
                else
                    participant.SendError(exception, false);
            }
            catch { }
            Application.Exit();

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
            participants = parts.ToArray();
            competitions.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new TutorialForm(competitions);

            processThread = new Thread(Process) { IsBackground = true };
            var timerThread = new Thread(TimeLimit) { IsBackground = true };

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            processThread.Start();
            timerThread.Start();


            Application.Run(form);
            
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            SendError((Exception)e.ExceptionObject);
        }
    }
}
