using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace DemoNetworkClient
{
    class Program
    {
        static void Main()
        {
            try
            {
                var tcpClient = new TcpClient("192.168.0.69", 14000);
                var streamReader = new StreamReader(tcpClient.GetStream());
                var streamWriter = new StreamWriter(tcpClient.GetStream());
                var team = streamReader.ReadLine();
                streamWriter.WriteLine("key");
                streamWriter.WriteLine("comp");
                streamWriter.Flush(); 
                var rand = new Random();
                while (true)
                {
                    streamWriter.WriteLine("<Command><Move>{0}</Move><Angle>{1}</Angle></Command>", rand.Next(-50, 100),
                                           180*(rand.NextDouble()*Math.PI - Math.PI/2)/Math.PI);
                    streamWriter.Flush();
                    string str = streamReader.ReadLine();
                    Console.WriteLine(str);
                    if (str == "end") return;
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
