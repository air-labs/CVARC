using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace MovementReconnectingClient
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 14000;

            while (true)
            {
                try
                {
                    Console.Write("Attempting to connect ");
                    var client = new CvarcClient<object, SimpleMovementCommand>();
                    client.Configurate(port, new ConfigurationProposal
                    {
                        LoadingData = new LoadingData { AssemblyName = "Demo", Level = "Movement" },
                        SettingsProposal = new SettingsProposal
                        {
                            TimeLimit = 3,
                            Controllers = new List<ControllerSettings>
                      {
                          new ControllerSettings { ControllerId="Left", Type= ControllerType.Client, Name="This" }
                      }
                        }
                    });
                    client.Act(SimpleMovementCommand.Move(10, 1));
                    client.Act(SimpleMovementCommand.Rotate(Angle.Pi, 1));
                    client.Act(SimpleMovementCommand.Exit());
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Thread.Sleep(1000);
            }
        }
    }
}
