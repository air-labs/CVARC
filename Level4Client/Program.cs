using System;
using System.Drawing;
using System.IO;
using ClientBase;
using CVARC.Basic.Controllers;
using CVARC.Network;
using RepairTheStarship.Sensors;

namespace Level4Client
{
    class Program
    {
        private static readonly ClientSettings Settings = new ClientSettings
        {
            BotName = Bots.Azura,
            Side = Side.Left,
            LevelName = LevelName.Level4,
            MapNumber = 5
        };

        private static void Main(string[] args)
        {
            var server = new CvarcClient(args, Settings).GetServer<ImageSensorsData>();
            var sensorData = server.Run();
            int i = 0;
            while (true)
            {
                var bytes = sensorData.Image.Bytes;
                var bitmap = new Bitmap(new MemoryStream(bytes));
                File.WriteAllBytes("images\\" + i++ + "image.jpeg", bytes);
                sensorData = server.SendCommand(Command.Sleep());
            }
            server.SendCommand(new Command { Time = 10000 });
        }
    }
}
