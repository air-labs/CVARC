using System;
using System.Net.Sockets;
using System.Threading;
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

        public static void RunScenarioServerBackground(Action<CvarcClient> serverScenario, Action<CvarcClient> clientScenario)
        {
            new Action<Action<CvarcClient>>(RunServerScenario).BeginInvoke(serverScenario, null, null);
            SingleClientScenario(clientScenario);
        }

        public static void RunScenarioClientBackground(Action<CvarcClient> serverScenario, Action<CvarcClient> clientScenario)
        {
            Action clientAction=()=>
                {
                    Thread.Sleep(500);
                    SingleClientScenario(clientScenario);
                };
            clientAction.BeginInvoke(null, null);
            RunServerScenario(serverScenario);
        }

        static byte[] message=new byte[] { 1,2,3};

        [TestMethod]
        public void ServerReceivesThenSends()
        {
            RunScenarioServerBackground(
                server =>
                {
                    var msg = server.ReadLine();
                    server.WriteLine(msg);
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
        public void ServerSendsThenReceives()
        {
            RunScenarioServerBackground(
                server =>
                {
                    server.WriteLine(message);
                    var data=server.ReadLine();
                    Assert.AreEqual(message.Length, data.Length);
                },
                client =>
                {
                    var msg = client.ReadLine();
                    client.WriteLine(msg);
                }
            );
        }

        public void ServerWaitsThenSend()
        {
            RunScenarioServerBackground(
               server =>
               {
                   Thread.Sleep(1000);
                   server.WriteLine(message);
             
               },
               client =>
               {
                   var msg = client.ReadLine();
                   Assert.AreEqual(message.Length, msg.Length);
               }
           );


        }

        public void ServerReceivesThenCloses()
        {
            RunScenarioServerBackground(
                server =>
                {
                    server.ReadLine();
                    server.Close();
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

        public void ServerReceivesThenDies()
        {
            RunScenarioServerBackground(
                server =>
                {
                    server.ReadLine();
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

        public void ClientSendsThenCloses()
        {
            RunScenarioClientBackground(
                server =>
                {
                    server.ReadLine();
                    try
                    {
                        server.WriteLine(message);
                        server.ReadLine();
                        Assert.Fail();
                    }
                    catch { }
                },
                client =>
                {
                    client.WriteLine(message);
                    client.Close();
                });
        }

        public void ClientSendsThenDies()
        {
            RunScenarioClientBackground(
                server =>
                {
                    server.ReadLine();
                    try
                    {
                        server.WriteLine(message);
                        server.ReadLine();
                        Assert.Fail();
                    }
                    catch { }
                },
                client =>
                {
                    client.WriteLine(message);
                });
        }


        

        public static void Main()
        {
            new MessagingTests().ClientSendsThenDies();

        }
    }
}
