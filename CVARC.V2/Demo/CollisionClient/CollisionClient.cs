
using System.Collections.Generic;
using CVARC.V2;
using CVARC.V2.SimpleMovement;
namespace CameraClient
{
    public class Program
    {


        static void Control(bool runServer)
        {
            var client = new CvarcClient<object, SimpleMovementCommand>();
            client.Configurate(runServer, 14000, new ConfigurationProposal
            {
                LoadingData = new LoadingData { AssemblyName = "Demo", Level = "Collision" },
                SettingsProposal = new SettingsProposal
                {
                    Controllers = new List<ControllerSettings>
                      {
                          new ControllerSettings { ControllerId="Left", Type= ControllerType.Client, Name="This" },
                          new ControllerSettings { ControllerId="Right", Type= ControllerType.Bot, Name="Backward" }
                      }
                }
            });
            client.Act(SimpleMovementCommand.Exit());
        }

        public static void Main(string[] args)
        {
            CVARC.V2.CVARCProgram.RunServerInTheSameThread(args, Control);
        }
    }
}