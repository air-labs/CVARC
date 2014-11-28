
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2.SimpleMovement;
namespace CameraClient
{
    public class Program
    {


        static void Control(int port)
        {
            var client = new CvarcClient<object, SimpleMovementCommand>();
            client.Configurate(port, new ConfigurationProposal
            {
                LoadingData = new LoadingData { AssemblyName = "Demo", Level = "Movement" },
                SettingsProposal = new SettingsProposal
                {
                    TimeLimit=3,
                    Controllers = new List<ControllerSettings>
                      {
                          new ControllerSettings { ControllerId="Left", Type= ControllerType.Client, Name="This" }
                      }
                }
            });
            client.Act(SimpleMovementCommand.Move(10, 1));
            client.Act(SimpleMovementCommand.Rotate(Angle.Pi, 1));
            client.Exit();
        }

        static void ControlWithSeparateProcess()
        {
            var port = 14000;
            var process = new Process
            {
                StartInfo =
                {
                    FileName = "CVARC.exe",
                    Arguments = "Debug " + port.ToString()
                }
            };
            process.Start();
            Thread.Sleep(500);
            Control(port);
        }

        public static void Main()
        {
            //CVARC.V2.CVARCProgram.RunServerInTheSameThread(Control);
            ControlWithSeparateProcess();
        }
    }
}