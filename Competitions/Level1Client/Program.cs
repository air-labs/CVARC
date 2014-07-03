using AIRLab.Mathematics;
using CVARC.Basic.Controllers;
using CVARC.Network;
using ClientBase;
using Gems.Sensors;
using StarshipRepair;

namespace Client
{
    internal class Program
    {
        private static readonly ClientSettings Settings = new ClientSettings
            {
                BotName = Bots.MolagBal,
                Side = Side.Right,
                LevelName = "Level1"
            };

        private static void Main(string[] args)
        {
            var server = new CVARKEngine(args, Settings).GetServer<SensorsData>();
            server.Run();
            server.GetSensorData(new Command { Angle = Angle.FromGrad(-90), Time = 1});
            server.GetSensorData(new Command { Move = 50, Time = 1 });
            server.GetSensorData(new Command { Action = "Grip", Time = 1 });
            server.GetSensorData(new Command { Move = -50, Time = 1 });
            server.GetSensorData(new Command { Angle = Angle.FromGrad(90), Time = 1 });
            server.GetSensorData(new Command { Action = "Release", Time = 1 });
            server.GetSensorData(new Command { Time = 10000 }); //TODO: сделать метод server.WaitForExit вот с таким телом. 
        }
    }
}
