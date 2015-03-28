using System.Drawing;
using System.IO;
using ClientBase;
using CommonTypes;
using CVARC.Basic.Controllers;
using CVARC.Network;
using RepairTheStarship.Sensors;

namespace Level4Client
{
    class Program
    {
        private const string Path = "C:\\images\\";
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
            var sensorData = server.Run().SensorsData;
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
            int i = 0;
            while (true)
            {
                File.WriteAllBytes(Path + i++ + "image.jpeg", sensorData.Image.Bytes);
                var bitmap = new Bitmap(new MemoryStream(sensorData.Image.Bytes));
                sensorData = server.SendCommand(FindRedDetail(bitmap));
            }
        }

        //todo
        private static Command FindRedDetail(Bitmap bitmap)
        {
            var q = 0;
            for (int i=0;i<bitmap.Width;i++)
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var pixel = bitmap.GetPixel(i, j);
                    if (pixel.R > 250 && pixel.B == 0 && pixel.G == 0)
                        q++;
                }
            return q ==0 ? Command.Rot(30) : Command.Sleep();
        }
    }
}
