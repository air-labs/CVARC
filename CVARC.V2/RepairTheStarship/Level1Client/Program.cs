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
                Console.WriteLine("X");
                action();
            }
        }

        static void Control()
        {
            Console.WriteLine("!");
            var client = new Level1Client();
            client.Configurate(true, RepairTheStarshipBots.Azura);
            client.Rotate(-90);
            client.Move(100);
            Console.WriteLine("!");
        }

        [STAThread]
        public static void Main(string[] args)
        {
            DebugRun(args,Control);
        }
    }
}
