using AIRLab.Mathematics;
using CVARC.Basic.Controllers;
using CVARC.Network;
using ClientBase;
using RepairTheStarship.Sensors;

namespace Client
{
    internal class Program
    {
        private static readonly ClientSettings Settings = new ClientSettings
            {
                BotName = Bots.None,
                Side = Side.Left,
                LevelName = "Level1"
            };

        private static void Main(string[] args)
        {
            var server = new CvarcClient(args, Settings).GetServer<SensorsData>();
            server.Run();
            server.SendCommand(new Command { Angle = Angle.FromGrad(-90), Time = 1 });
            server.SendCommand(new Command { Move = 50, Time = 1 });
            server.SendCommand(new Command { Action = CommandAction.Grip, Time = 1 });
            server.SendCommand(new Command { Move = -50, Time = 1 });
            server.SendCommand(new Command { Angle = Angle.FromGrad(90), Time = 1 });
            server.SendCommand(new Command { Action = CommandAction.Release, Time = 1 });
            server.SendCommand(new Command { Time = 10000 }); //TODO: сделать метод server.WaitForExit вот с таким телом. 
        }
    }
}
