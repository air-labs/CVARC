using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2.SimpleMovement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RepairTheStarship;

namespace Level1Example
{
    class Program
    {
        static void DebugRun(string[] args, Action action)
        {
            if (args.Length == 0)
            {
                new Action(() =>
                {
                    Thread.Sleep(1000);
                    action();
                }).BeginInvoke(null, null);
                CVARCProgram.Main(new string[] { "Debug", "14000" });
            }
            else
            {
                action();
            }
        }

        static void Control()
        {
            var client = new Level1Client();
            client.Configurate(true, RepairTheStarshipBots.Azura);
            for (int i = 0; i < 10; i++)
            {
                var sensors = client.Rotate(90);
                Console.WriteLine(sensors.RobotId);
                sensors = client.Move(50);
                Console.WriteLine(sensors.RobotId);
            }
            client.Exit();
        }

        [STAThread]
        public static void Main(string[] args)
        {
            DebugRun(args,Control);
        }
    }
}
