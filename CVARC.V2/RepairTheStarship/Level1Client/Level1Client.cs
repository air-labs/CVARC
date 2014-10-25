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

        static void PrintLocation(Level1SensorData sensors)
        {
            var location=sensors.SelfLocation;
            Console.WriteLine("{0} {1}", location.X, location.Y);

        }

        static void Control(bool runServer)
        {
            var client = new Level1Client();
            client.Configurate(runServer, true);
            for (int i = 0; i < 10; i++)
            {
                var sensors = client.Rotate(90);
                PrintLocation(sensors);
                sensors = client.Move(50);
                PrintLocation(sensors);
            }
            client.Exit();
        }

        [STAThread]
        public static void Main(string[] args)
        {
            Control(args.Length == 0);
        }
    }
}
