using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2.SimpleMovement;
using Demo;

namespace CameraClient
{
    public class Program
    {
        static void Control(bool runServer)
        {
            var client = new CvarcClient<SensorsWithCamera, SimpleMovementCommand>();
            client.Configurate(runServer, 14000, new ConfigurationProposal
            {
                LoadingData = new LoadingData { AssemblyName = "Demo", Level = "Camera" },
                SettingsProposal = new SettingsProposal
                {
                    Controllers = new List<ControllerSettings>
                      {
                          new ControllerSettings { ControllerId="Left", Type= ControllerType.Client, Name="This" },
                          new ControllerSettings { ControllerId="Right", Type= ControllerType.Bot, Name="Random" }
                      }
                }
            });
            for (int i = 0; i < 100; i++)
            {
                var sensorsData = client.Act(new SimpleMovementCommand { AngularVelocity = Angle.FromGrad(30), Duration = 1 });
            }
        }

        public static void Main(string[] args)
        {
            CVARC.V2.CVARCProgram.RunServerInTheSameThread(args, Control);
        }
    }
}
