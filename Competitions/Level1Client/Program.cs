using AIRLab.Mathematics;
using CVARC.Basic.Controllers;
using CVARC.Network;
using ClientBase;
using Gems.Sensors;

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
            var server = new CVARKEngine(args, Settings).GetServer<SensorsData>();
            server.Run();
            server.GetSensorData(new Command { Angle = Angle.FromGrad(-90), Time = 1});
            server.GetSensorData(new Command { Move = 50, Time = 1 });
            server.GetSensorData(new Command { Cmd = Cmd.Grip, Time = 1 });
            server.GetSensorData(new Command { Move = -50, Time = 1 });
            server.GetSensorData(new Command { Angle = Angle.FromGrad(90), Time = 1 });
            server.GetSensorData(new Command { Cmd = Cmd.Release, Time = 1 });
        }
    }
}
