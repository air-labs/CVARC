/*using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AIRLab.Thornado;
using CVARC.Core.Replay;

namespace CVARC.Basic.Controllers
{
    public class NetworkController : Controller
    {
        private readonly World _wrld;
        private TcpListener _serv;

        public NetworkController(World wrld)
        {
            _wrld = wrld;
            StartMatch += () => new Thread(() =>
                                               {
                                                   Thread.Sleep(90000);
                                                   Stop();
                                               }).Start();
        }

        private int cls = 0;
        public void Run(int port)
        {
            _serv = new TcpListener(IPAddress.Any, port);
            _serv.Start();
            try
            {
                for (int i = 0; i < _wrld.RobotCount; ++i)
                {
                    var cl1 = _serv.AcceptTcpClient();
                    var n = i;
                    cls++;
                    new Thread(() => WorkWithClient(cl1, n)).Start();
                }
                Ready = true;
                StartMatch();
            }
            catch
            {
            }
        }

        public event Action StartMatch = () => { };

        private void WorkWithClient(TcpClient client, int number)
        {
            try
            {
                var sr = new StreamReader(client.GetStream());
                var sw = new StreamWriter(client.GetStream());
                while (!Ready)
                {
                    Thread.Sleep(10);
                }
                sw.WriteLine(number);
                sw.Flush();
                var key = sr.ReadLine();
                var competitions = sr.ReadLine();
                while (!Exit && client.Connected)
                {
                    var input = sr.ReadLine();
                    var cmd = IO.XML.ParseString<Command>(input);
                    RaiseProcess(new[] {cmd});
                    ProcessReply(sw, _wrld, _wrld.Robots[number]);
                    sw.Flush();
                }
                sw.WriteLine("end");
                sw.Flush();
                try
                {
                    using (var wb = new WebClient())
                    {
                        var data = new NameValueCollection();
                        data["key"] = key;
                        data["competitions"] = competitions;
                        data["robotNumber"] = number.ToString();
                        data["results"] = "[" +
                                          string.Join(",",
                                                      Enumerable.Range(0, _wrld.RobotCount)
                                                                .Select(
                                                                    a =>
                                                                    "{num:" + a + ", score: " +
                                                                    _wrld.Score.GetFullSumForRobot(a) + "}")) + "]";
                        var replay = ConverterToJavaScript.Convert(Logger.SerializationRoot);
                        data["log"] = replay;
                        sw.WriteLine(
                            Encoding.UTF8.GetString(wb.UploadValues("http://air-labs.ru/match/save", "POST", data)));
                    }
                }
                catch (Exception e)
                {
                }
                sw.Close();
                sr.Close();
                client.Close();
                cls--;
            }
            catch (Exception e)
            {
                Environment.Exit(1);
            }
        }

        public void ProcessReply(StreamWriter sw, World world, Robot team)
        {
            try
            {
                sw.Write("<d>");
                team.Sensors.ForEach(a =>
                                         {
                                             var str = a.Measure();
                                             sw.Write("<" + a.Name + ">");
                                             sw.Write(str.Replace('\r',' ').Replace('\n',' '));
                                             sw.Write("</" + a.Name + ">");
                                         });
                sw.WriteLine("</d>");
            }
            catch (Exception e)
            {
                Environment.Exit(1);
            }
        }

        protected bool Exit { get; set; }

        protected bool Ready { get; set; }

        public ReplayLogger Logger { get; set; }

        public void Stop()
        {
            Exit = true;
            _serv.Stop();
            while (cls != 0)
                Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }

}
*/