using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;

namespace DemoNetworkClient
{
    class Program
    {
         const double MaxLinearVelocity=10;
        const double MaxAngularVelocity=10;


        static StreamReader streamReader;
        static StreamWriter streamWriter;

        static void ReadAndPrint()
        {
            var line = streamReader.ReadLine();
            var doc = XDocument.Parse(line);
            Console.WriteLine(doc.ToString());

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
            var tcpClient = new TcpClient("213.159.198.23", 14000);
            streamReader = new StreamReader(tcpClient.GetStream());
            streamWriter = new StreamWriter(tcpClient.GetStream());


            streamWriter.WriteLine("<Hello><AccessKey>12345</AccessKey><Side>Left</Side><Opponent>Simple</Opponent></Hello>");
            streamWriter.Flush();
            ReadAndPrint();
            Rot(-90);
            ReadAndPrint();
            Mov(50);
            ReadAndPrint();
            Rot(90);
            ReadAndPrint();
            Mov(200);
            ReadAndPrint();
            ReadAndPrint();

            Console.ReadKey();
        }
    }
}
