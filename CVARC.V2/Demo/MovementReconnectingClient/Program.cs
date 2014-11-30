using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2.SimpleMovement;
using Demo;

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

                    var loadingData = new LoadingData { AssemblyName = "Demo", Level = "Movement" };

                    var settings = new SettingsProposal
                    {
                        TimeLimit=3,
                        Controllers = new List<ControllerSettings> 
                        {
                            new ControllerSettings
                            {
                                 ControllerId=MovementLogicPart.ControllerId, Name="This", Type= ControllerType.Client
                            }
                        }
                    };
                    
                    var state=new MovementWorldState();
                
                    client.Configurate(port, new ConfigurationProposal { LoadingData=loadingData, SettingsProposal=settings}, state);

                    client.Act(SimpleMovementCommand.Move(10, 3));
                    client.Act(SimpleMovementCommand.Rotate(Angle.Pi, 1));
                    client.Exit();
                    Console.WriteLine("Success");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Thread.Sleep(1000);
            }
        }
    }
}