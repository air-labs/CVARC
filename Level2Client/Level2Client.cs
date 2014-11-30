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
using System.Windows.Forms;

namespace Level2Example
{
    class Program
    {

        static void PrintLocation(Level1SensorData sensors)
        {
            var location = sensors.SelfLocation;
            Console.WriteLine("{0} {1}", location.X, location.Y);

        }

        static Level2ClientForm form;

        static void Control(int port)
        {
            var client = new Level3Client();
            client.Configurate(port, true);
            client.Rotate(-90);
            client.Move(100);
            client.Rotate(90);
            client.Move(100);
            for (int i = 0; i < 100; i++)
            {
                var sensors = client.Rotate(10);
                form.ShowMap(sensors.Map);
            }
            client.Exit();
        }

        static void Run(int port)
        {
            form = new Level2ClientForm();
            new Action<int>(Control).BeginInvoke(port, null, null);
            Application.Run(form);
        }



        [STAThread]
        public static void Main(string[] args)
        {
            int port = 14000;
            if (args.Length == 0)
            {
                Level1Client.StartKrorServer(port);
            }
            else
            {
                port = int.Parse(args[0]);
            }

            Run(port);
        }
    }
}