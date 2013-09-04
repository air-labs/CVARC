using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LocalNetworkClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Process();
            p.StartInfo.FileName = "..\\..\\..\\..\\CVARC\\CVARC.Network\\bin\\Debug\\CVARC.Network.exe";
            p.StartInfo.Arguments= "..\\..\\..\\..\\Competitions\\Fall2013.0\\bin\\Debug\\Fall2013.0.dll";
            var file = new FileInfo(p.StartInfo.FileName);
            Console.WriteLine(file.FullName);
            p.Start();
            Thread.Sleep(2000);
            var tcpClient = new TcpClient("127.0.0.1", 14000);
            var streamReader = new StreamReader(tcpClient.GetStream());
            var streamWriter = new StreamWriter(tcpClient.GetStream());


            streamWriter.WriteLine("<Hello><AccessKey>1234</AccessKey><Side>Left</Side></Hello>");
            streamWriter.Flush();
        
            var rand = new Random();

            for (int i=0;i<90;i++)
            {
                Console.WriteLine(streamReader.ReadLine());
                var line=String.Format("<Command><LinearVelocity>{0}</LinearVelocity><AngularVelocity>{1}</AngularVelocity><Time>1</Time></Command>", rand.Next(-50, 100),
                                       180 * (rand.NextDouble() * Math.PI - Math.PI / 2) / Math.PI);
                new Gems.GemsNetworkController().ParseRequest(line);
                streamWriter.WriteLine(line);
                streamWriter.Flush();
            }
        }
    }
}
