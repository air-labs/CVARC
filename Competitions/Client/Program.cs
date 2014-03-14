using System;
using CVARC.Network;

namespace Client
{
    internal class Program
    {
        readonly static ClientSettings Settings = new ClientSettings();

        //Зайдите в файл SimpleClient.exe.config, чтобы поменять настройки.
        private static void Main(string[] args)
        {
            var server = new CVARKEngine(args, Settings).GetServer();
            var sensorsData = server.Run(GetHelloPackage());
            while (true)
            {
                var data = server.GetSensorData();
                Console.WriteLine(data);
            }
        }

        private static HelloPackage GetHelloPackage()
        {
            return new HelloPackage
                {
                    MapSeed = Settings.MapSeed,
                    Opponent = Settings.BotName,
                    Side = Side.Left
                };
        }
    }
}
