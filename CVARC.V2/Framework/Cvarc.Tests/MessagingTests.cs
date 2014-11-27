using System;
using System.Net.Sockets;
using CVARC.V2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cvarc.Tests
{
    [TestClass]
    public class MessagingTests
    {
        public static void RunServerScenario(Action<CvarcClient> scenario)
        {
            var listener = new TcpListener(14000);
            listener.Start();
            var client = listener.AcceptTcpClient();
            var cvarcClient = new CvarcClient(client);
            scenario(cvarcClient);
        }

        public static void SingleClientScenario(Action<CvarcClient> scenario)
        {
            var client = new TcpClient();
            client.Connect("127.0.0.1", 14000);
            var cvarcClient = new CvarcClient(client);
            scenario(cvarcClient);
        }

        public static void RunClientServerScenario(Action<CvarcClient> serverScenario, Action<CvarcClient> clientScenario)
        {
            new Action<Action<CvarcClient>>(RunServerScenario).BeginInvoke(serverScenario, null, null);
            SingleClientScenario(clientScenario);
        }

        static byte[] message=new byte[] { 1,2,3};

        [TestMethod]
        public void SendThenReceive()
        {
            RunClientServerScenario(
                client =>
                {
                    var msg = client.ReadLine();
                    client.WriteLine(msg);
                },
                client =>
                {
                    client.WriteLine(message);
                    var data=client.ReadLine();
                    Assert.AreEqual(message.Length, data.Length);
                }
            );
        }

        [TestMethod]
        public void ReceiveThenSend()
        {
            RunClientServerScenario(
                client =>
                {
                    client.WriteLine(message);
                    var data=client.ReadLine();
                    Assert.AreEqual(message.Length, data.Length);
                },
                client =>
                {
                    var msg = client.ReadLine();
                    client.WriteLine(msg);
                }
            );
        }

        public void ReceiveThenClose()
        {
            RunClientServerScenario(
                client =>
                {
                    client.ReadLine();
                    client.Close();
                },
                client =>
                {
                    client.WriteLine(message);
                    try
                    {
                        client.ReadLine();
                        Assert.Fail();
                    }
                    catch { }
                });
        }

        public static void Main()
        {
            new MessagingTests().ReceiveThenClose();

        }
    }
}
