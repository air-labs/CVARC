﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using CVARC.Basic;

namespace CVARC.Network
{
    public static class Program
    {
        static NetworkParticipant participant;
        static Participant[] participants;
        static Form form;
        private static bool realTime = true;

        [STAThread]
        static void Main()
        {
            InternalMain(new NetworkSettings());
        }

        public static void DebugMain(NetworkSettings networkSettings)
        {
            InternalMain(networkSettings);
        }

        private static void InternalMain(NetworkSettings settings)
        {
            try
            {
                InitCompetition(settings);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                form = new TutorialForm(participant.competitionsBundle.competitions);
                new Thread(() => participant.competitionsBundle.competitions.ProcessParticipants(realTime, 1000, participants))
                {
                    IsBackground = true
                }.Start();
                Application.Run(form);
            }
            catch (Exception e)
            {
                throw;//todo Убрать
                MessageBox.Show(e.Message, "CVARC Network", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private static void InitCompetition(NetworkSettings settings)
        {
            participants = new Participant[2];
            participant = new NetworkParticipant(settings.CompetitionsName);
            participants[participant.ControlledRobot] = participant;
            var botNumber = participant.ControlledRobot == 0 ? 1 : 0;
            participant.competitionsBundle.competitions.Initialize(new CVARCEngine(participant.competitionsBundle.Rules),
                new[] { new RobotSettings(participant.ControlledRobot, false), new RobotSettings(botNumber, true) });
            var botName = participant.HelloPackage.Opponent ?? "None";
            participants[botNumber] = participant.competitionsBundle.competitions.CreateBot(botName, botNumber);
            if (settings.StartClient)
                StartClient();
        }

        private static void StartClient()
        {
            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "..\\..\\..\\..\\Competitions\\DemoNetworkClient\\bin\\Debug\\DemoNetworkClient.exe";
            p.Start();
        }
    }
}
