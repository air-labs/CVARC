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
        static StreamReader clientReader;
        static StreamWriter clientWriter;
        static int controlledRobot = 0;
        static HelloPackage hello;
        static Competitions competitions;
        static Form form;



        static void Process()
        {
            double time = competitions.GameTimeLimit;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                var reply = "<Sensors>";
                foreach (var e in competitions.World.Robots[controlledRobot].Sensors)
                    reply += e.Measure();
                reply += "</Sensors>";
                reply = reply.Replace("\r", "").Replace("\n", "");
                clientWriter.WriteLine(reply);
                clientWriter.Flush();

                var request = clientReader.ReadLine();
                Console.WriteLine(request);
                var command = competitions.NetworkController.ParseRequest(request);

                competitions.Behaviour.ProcessCommand(competitions.World.Robots[controlledRobot], command);


                competitions.MakeCycle(Math.Min(time, command.Time), false);
                time -= command.Time;
                if (time < 0) break;
                
            }
         
            //послать лог на сервак и вернуть ссылку
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

            Console.Write("Starting server... ");
                var listener = new TcpListener(IPAddress.Any, 14000);
                listener.Start();
                Console.WriteLine("OK");


                Console.Write("Waiting for client... ");
                var client = listener.AcceptTcpClient();
                clientReader = new StreamReader(client.GetStream());
                clientWriter = new StreamWriter(client.GetStream());
                Console.WriteLine("OK");

                Console.Write("Receiving hello package... ");
                var line = clientReader.ReadLine();
                var document = XDocument.Parse(line);
                hello = new HelloPackage();
                hello.Parse(document);
                Console.WriteLine("OK");
        

            controlledRobot = hello.LeftSide ? 0 : 1;

            competitions.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new TutorialForm(competitions);

            new Thread(Process) { IsBackground = true }.Start();

            Application.Run(form);
            
        }
    }
}
