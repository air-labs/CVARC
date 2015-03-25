using System;
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
                LevelName = LevelName.Level1
            };

        private static void Main(string[] args)
        {
            var server = new CvarcClient(args, Settings).GetServer<BaseSensorData>();
            var helloPackageAns = server.Run();
            Console.WriteLine("Your Side: {0}", helloPackageAns.RealSide);
            server.SendCommand(new Command { AngularVelocity = Angle.FromGrad(-90), Time = 1 });
            server.SendCommand(new Command { LinearVelocity = 50, Time = 1 });
            server.SendCommand(new Command { Action = CommandAction.Grip, Time = 1 });
            server.SendCommand(new Command { LinearVelocity = -50, Time = 1 });
            server.SendCommand(new Command { AngularVelocity = Angle.FromGrad(90), Time = 1 });
            server.SendCommand(new Command { Action = CommandAction.Release, Time = 1 });
			server.Exit();
        }
    }
}
