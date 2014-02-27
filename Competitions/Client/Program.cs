using System;
using CVARC.Network;

namespace Client
{
    class Program
    {
        //Зайдите в файл SimpleClient.exe.config, чтобы поменять настройки.
        static void Main(string[] args)
        {
            var server = new CVARKEngine(args).GetServer();
            server.Run(new HelloPackage { MapSeed = 1 });
            while (true)
            {
                var data = server.GetSensorData();
                Console.WriteLine(data);
            }
        }
    }
}
