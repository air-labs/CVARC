using System;
using System.Threading;
using System.Windows.Forms;
using CVARC.Basic;

namespace CVARK.Network
{
    static class Program
    {
        static bool isLocalServer = false;
        static Competitions competitions;
        static NetworkParticipant participant;
        static Participant[] participants;
        static Form form;
        static Thread processThread;
        static bool goodByeFlag = false;

        static void Process()
        {
            competitions.ProcessParticipants(isLocalServer, 1000, participants);
            var replayId = "";
            if (!isLocalServer) replayId = competitions.SendPostReplay(participant.HelloPackage.AccessKey, participant.ControlledRobot);
            participant.SendReplay(replayId);
            Application.Exit();
        }

        private static void SendError(Exception exception)
        {
            if (participant == null) return;
            if (exception is UserInputException)
                participant.SendError(exception.InnerException, true);
            else
                participant.SendError(exception, false);
            participant = null;
        }

        private static void AcceptHelloPackage()
        {
            participants = new Participant[2];
            participant = new NetworkParticipant(competitions);
            participants[participant.ControlledRobot] = participant;
            Console.WriteLine(participant.HelloPackage.Side.ToString());

            var botNumber = participant.ControlledRobot == 0 ? 1 : 0;
            var botName = participant.HelloPackage.Opponent;
            if (botName == null) botName = "None";
            if (!competitions.BotIsAvailable(botName))
                throw new UserInputException("The opponent's name is not valid");

            participants[botNumber] = competitions.CreateBot(botName, botNumber);
        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                var settings = new NetworkSettings();
                competitions = Competitions.Load(settings);
                isLocalServer = settings.IsLocalServer;
                if (!isLocalServer)
                    StartDebugServer();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "CVARC Network", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
//            try
//            {
                AcceptHelloPackage(); //Why CurrentDomain.UnhandledException does not work for this one???
//            }
//            catch (Exception e)
//            {
//                SendError(e);
//                return;
//            }

            competitions.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new TutorialForm(competitions);

            processThread = new Thread(Process) { IsBackground = true };

            processThread.Start();
            Application.Run(form);
            
        }

        private static void StartDebugServer()
        {
            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "..\\..\\..\\..\\Competitions\\DemoNetworkClient\\bin\\Debug\\DemoNetworkClient.exe";
            p.Start();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            SendError((Exception)e.ExceptionObject);
        }
    }
}
