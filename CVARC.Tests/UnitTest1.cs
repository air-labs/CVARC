using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CVARC.V2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SocketsTest
{
    [TestClass]
    public class NetworkTests
    {
        //public CvarcClient Connect()
        //{
        //    var client = new TcpClient();
        //    client.Connect("127.0.0.1", 14000);
        //    return new CvarcClient(client);
        //}

        //public void RunServerScenario(Action<CvarcClient> scenario)
        //{
        //    var listener = new TcpListener(14000);
        //    var client = listener.AcceptTcpClient();
        //    var cvarcClient = new CvarcClient(client);
        //    scenario(cvarcClient);
        //}

        //public void RunClientServerScenario(Action<CvarcClient> serverSide, Action clientSide)
        //{
        //    new Action<Action<CvarcClient>>(RunServerScenario).BeginInvoke(serverSide, null, null);
        //    Thread.Sleep(100);
        //    clientSide();
        //}

        readonly byte[] message = new byte[] { 1, 2, 3, 4, 5 };

        [TestMethod]
        public void Test1()
        {
            

        }

    }
}
