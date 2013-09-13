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
        static bool localServer = false;
        static Competitions competitions;
        static NetworkParticipant participant;
        static Participant[] participants;
        static Form form;
        static Thread processThread;
        static bool goodByeFlag = false;

        static void Process()
        {
            competitions.ProcessParticipants(localServer, 1000, participants);
            var replayId = "";
            //if (!localServer) competitions.SendPostReplay(participant.HelloPackage.AccessKey, 0, participant.ControlledRobot);
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
                participant = null;
            }
            catch { }
            Application.Exit();

        }


        static void AcceptHelloPackage()
        {
            participants = new Participant[2];
            participant = new NetworkParticipant(competitions);
            participants[participant.ControlledRobot] = participant;
            Console.WriteLine(participant.HelloPackage.Side.ToString());

            var botNumber = participant.ControlledRobot==0 ? 1 : 0;
            var botName = participant.HelloPackage.Opponent;
            if (botName == null) botName = "None";
            if (!competitions.BotIsAvailable(botName))
                throw new UserInputException("The opponent's name is not valid");
           
            participants[botNumber]=competitions.CreateBot(botName, botNumber);


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

            if (args.Length > 1)
            {
                if (args[1]=="-local") localServer=true;
                if (args[1] == "-debug")
                {
                    localServer = true;
                    var p = new System.Diagnostics.Process(); 
                    p.StartInfo.FileName = "..\\..\\..\\..\\Competitions\\DemoNetworkClient\\bin\\Debug\\DemoNetworkClient.exe";
                    p.Start();
                }
            }

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                AcceptHelloPackage(); //Why CurrentDomain.UnhandledException does not work for this one???
            }
            catch (Exception e)
            {
                SendError(e);
                return;
            }

            competitions.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new TutorialForm(competitions);

            processThread = new Thread(Process) { IsBackground = true };


            processThread.Start();
            Application.Run(form);
            
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            SendError((Exception)e.ExceptionObject);
        }
    }
}
