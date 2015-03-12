using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AIRLab.Mathematics;
using CVARC.V2;
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
                    var client = new CvarcClient<object, MoveAndGripCommand>();

                    var loadingData = new LoadingData { AssemblyName = "Demo", Level = "Movement" };

                    var settings = new SettingsProposal
                    {
                        TimeLimit=3,
                        Controllers = new List<ControllerSettings> 
                        {
                            new ControllerSettings
                            {
                                 ControllerId=TwoPlayersId.Left, Name="This", Type= ControllerType.Client
                            }
                        }
                    };
                    
                    var state=KnownWorldStates.EmptyWithOneRobot(false);
                    var rules=new MoveAndGripRules();
                
                    client.Configurate(port, new ConfigurationProposal { LoadingData=loadingData, SettingsProposal=settings}, state);

                    client.Act(rules.Move(30));
                    client.Act(rules.Rotate(Angle.Pi));
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