
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AIRLab.Mathematics;
using CVARC.V2;
using Demo;
namespace CameraClient
{
    public class Program
    {


        static void Control(int port)
        {
            var client = new CvarcClient<DemoSensorsData, MoveAndGripCommand>();
            client.Configurate(port, new ConfigurationProposal
            {
                LoadingData = new LoadingData { AssemblyName = "Demo", Level = "Demo" },
                SettingsProposal = new SettingsProposal
                {
                    TimeLimit=3,
                    Controllers = new List<ControllerSettings>
                      {
                          new ControllerSettings { ControllerId="Left", Type= ControllerType.Client, Name="This" },
                          new ControllerSettings { ControllerId="Right", Type= ControllerType.Bot, Name="Stand" }
                      }
                }
            }, KnownWorldStates.EmptyWithOneRobot(false));
            var rules = new MoveAndGripRules();
            var sensors = client.Act(rules.Move(10));
            sensors = client.Act(rules.Rotate(Angle.Pi));
            client.Exit();
        }

        static void ControlWithSeparateProcess()
        {
            var port = 14345;
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
            CVARC.V2.CVARCProgram.RunServerInTheSameThread(Control);
            //ControlWithSeparateProcess();
        }
    }
}