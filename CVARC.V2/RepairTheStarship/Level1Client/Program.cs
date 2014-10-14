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

namespace Level1Client
{
    class Program
    {

        static void StartCvarc()
        {
            try
            {
                CVARC.V2.CVARCProgram.Main(new string[] { "-Debug", "-Port", "14000" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey(false);
            }
        }

        static void RunClient()
        {
            var client = new TcpClient();
            client.Connect("127.0.0.1", 14000);
            var grobo = new CvarcTcpClient(client);

            var config = new Configuration()
            {
                Assembly = "RepairTheStarship",
                Level = "Level1",
                Controllers = 
                { 
                    new ControllerConfiguration { ControllerId="Left",  Type = ControllerType.Client, Name="This" },
                    new ControllerConfiguration { ControllerId="Right",  Type = ControllerType.Bot, Name="Azura" }
                }
            };


            grobo.SerializeAndSend(config);
            var data = grobo.ReadObject<RepairTheStarship.Level1SensorData>();
            grobo.SerializeAndSend(new SimpleMovementCommand { AngularVelocity = Angle.FromGrad(90), Duration = 1 });
      
        }

        public static void Main()
        {
            new Action(RunClient).BeginInvoke(null, null);
            StartCvarc();
        }
    }
}
