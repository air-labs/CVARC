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


            streamWriter.WriteLine("<Command><LinearVelocity>{0}</LinearVelocity><AngularVelocity>{1}</AngularVelocity><Time>{2}</Time></Command>", 0, -10, 9);
            streamWriter.WriteLine("<Command><LinearVelocity>{0}</LinearVelocity><AngularVelocity>{1}</AngularVelocity><Time>{2}</Time></Command>", 10, 0, 5);

            streamWriter.Flush();
            Console.ReadKey();
            
        }
    }
}
