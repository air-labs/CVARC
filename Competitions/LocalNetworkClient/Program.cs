
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
using StarshipRepair;

namespace LocalNetworkClient
{
    class Program
    {
        static double MaxLinearVelocity { get { return SRCompetitions.MaxLinearVelocity; } }
        static double MaxAngularVelocity { get { return SRCompetitions.MaxAngularVelocity; } }


        static StreamReader streamReader;
        static StreamWriter streamWriter;

        static void ReadAndPrint()
        {
            var line = streamReader.ReadLine();
            var doc = XDocument.Parse(line);
            Console.WriteLine(doc.ToString());

        }

        static void ReadAngle()
        {
            var line = streamReader.ReadLine();
            var doc = XDocument.Parse(line);
            var angle = doc.Elements()
                                .Where(z => z.Name == "Sensors")
                                .FirstOrDefault().Elements()
                                .Where(z => z.Name == "Robots")
                                .FirstOrDefault()
                                .Elements()
                                .Where(z => z.Name == "Robot")
                                .FirstOrDefault();
                                //.Where(
                                //    z => z.Elements().Where(x=>x.Name == "Number").FirstOrDefault().Value == "0"
                                //        )
                                //.FirstOrDefault();

            Console.WriteLine(angle.ToString());

        }

        static void Mov(double distance)
        {

            streamWriter.WriteLine("<Command><LinearVelocity>{0}</LinearVelocity><Time>{1}</Time></Command>", 
                    Math.Sign(distance)*MaxLinearVelocity, 
                    Math.Abs(distance)/MaxLinearVelocity);
            streamWriter.Flush();
        }

        static void Rot(double angle)
        {
              streamWriter.WriteLine("<Command><AngularVelocity>{0}</AngularVelocity><Time>{1}</Time></Command>", 
                    Math.Sign(angle)*MaxAngularVelocity, 
                    Math.Abs(angle)/MaxAngularVelocity);
            streamWriter.Flush();
        }

        static void Cmd(string cmd)
        {
            streamWriter.WriteLine("<Command><Action>{0}</Action></Command>", 
                    cmd);
            streamWriter.Flush();
        }

        static void Main(string[] args)
        {
            var p = new Process();
            p.StartInfo.FileName = "..\\..\\..\\..\\CVARC\\CVARC.Network\\bin\\Debug\\CVARC.Network.exe";
            p.StartInfo.Arguments = "..\\..\\..\\..\\Competitions\\Fall2013.0\\bin\\Debug\\Fall2013.0.dll -local";
            var file = new FileInfo(p.StartInfo.FileName);
            p.Start();
            Thread.Sleep(1000);
            var tcpClient = new TcpClient("127.0.0.1", 14000);
            streamReader = new StreamReader(tcpClient.GetStream());
            streamWriter = new StreamWriter(tcpClient.GetStream());


            streamWriter.WriteLine("<Hello><AccessKey>1234</AccessKey><Side>Random</Side><Opponent>MolagBal</Opponent></Hello>");
            streamWriter.Flush();

            ReadAndPrint();
            ReadAndPrint();

            Console.ReadKey();
        }
    }
}
