using AIRLab.Mathematics;
using CVARC.V2;
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

namespace ClientExample
{
    class Program
    {

        static void PrintLocation(CommonSensorData sensors)
        {
            var location = sensors.SelfLocation;
            Console.WriteLine("{0} {1}", location.X, location.Y);

        }

        static ClientForm form;

        static void Control(int port)
        {
            var client = new Level2Client();
			client.SensorDataReceived += sensorData => form.ShowMap(sensorData.Map);
			client.Configurate(port, true, RepairTheStarshipBots.MolagBal);
			client.Rotate(-90);
            client.Move(100);
            client.Rotate(90);
            client.Move(110);
            for (int i = 0; i < 10; i++)
            {
                client.Rotate(10);
            }
            client.Exit();
        }

        static void Run(int port, bool debug)
        {
            form = new ClientForm();
			new Thread(
				() =>
				{
					if (debug)
					{
						Control(port);
						return;
					}
					try
					{
						Control(port);
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
					}
				}).Start();
            Application.Run(form);
        }

		static void Run(int port)
		{
			Run(port, true);
		}

		
        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
				CVARC.V2.CVARCProgram.RunServerInTheSameThread(Run);
            }
            else
            {
				var port = int.Parse(args[0]);
				Run(port, false);
            }
        }

    }
}