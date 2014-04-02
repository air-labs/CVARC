using System;
using CVARC.Network;
using ClientBase;
using Gems.Sensors;

namespace Client
{
    internal class Program
    {
        private static readonly ClientSettings Settings = new ClientSettings
            {
                MapSeed = 1,
                BotName = Bots.None,
                Side = Side.Left
            };

        private static void Main(string[] args)
        {
            var server = new CVARKEngine(args, Settings).GetServer<SensorsData>();
            var sensorsData = server.Run();
            while (true)
            {
                Console.WriteLine(sensorsData);
                sensorsData = server.GetSensorData();
            }
        }
    }
}
