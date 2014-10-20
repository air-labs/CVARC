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
            var location = sensors.RobotsLocations.Single(z => z.Id == sensors.RobotId);
            Console.WriteLine("{0} {1}", location.X, location.Y);

        }

        static Level2ClientForm form;

        static void Control(bool runServer)
        {
            var client = new Level3Client();
            client.Configurate(runServer, true);
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

        static void Run(bool runServer)
        {
            form = new Level2ClientForm();
            new Action<bool>(Control).BeginInvoke(runServer, null, null);
            Application.Run(form);
        }

            

        [STAThread]
        public static void Main(string[] args)
        {
           // Run(args.Length == 0);
            CVARC.V2.CVARCProgram.RunServerInTheSameThread(args, Run);
        }
    }
}
