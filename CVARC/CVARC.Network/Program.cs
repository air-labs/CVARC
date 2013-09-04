using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using CVARC.Basic;
using CVARC.Basic.Controllers;

namespace CVARK.Network
{


    static class Program
    {
        static StreamReader clientReader;

        static void ReadPackage<T>()
        {
            var line = clientReader.ReadLine();
            var document = XDocument.Load(line);
            Console.WriteLine(document.ToString());

        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                MessageBox.Show("Please specify the assembly with rules", "CVARC Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var competitions = Competitions.Load(args[0]);

            Console.Write("Starting server... ");
            var listener = new TcpListener(IPAddress.Any, 14000);
            listener.Start();
            Console.WriteLine("OK");
            
            
            Console.Write("Waiting for client... ");
            var client = listener.AcceptTcpClient();
            clientReader = new StreamReader(client.GetStream());
            Console.WriteLine("OK");

            Console.Write("Receiving hello package... ");
            ReadPackage<string>();
            Console.WriteLine("OK");

            Console.ReadKey();
            
        }
    }
}
