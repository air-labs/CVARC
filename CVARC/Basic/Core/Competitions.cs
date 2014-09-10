using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using AIRLab.Mathematics;
using CVARC.Basic.Controllers;
using CVARC.Core;
using CVARC.Network;
using AIRLab;

namespace CVARC.Basic
{
    public abstract class Competitions
    {
        public IEngine Engine { get; private set; }
        public double GameTimeLimit { get; protected set; }
        public double NetworkTimeLimit { get; protected set; }
        public virtual double LinearVelocityLimit { get { return 10; } }
        public virtual Angle AngularVelocityLimit { get { return Angle.FromGrad(30); } }
        public Dictionary<string, Type> AvailableBots { get; private set; }

        public ScoreCollection Score { get; private set; }
        public ISceneSettings Settings { get; private set; }
        public HelloPackage HelloPackage { get; set; }
        public abstract ISceneSettings ParseSettings(HelloPackage helloPackage);
        public abstract Robot CreateRobot(int robotNumber);
        public abstract Robot CreateBot(int robotNumber);
        public List<Robot> Robots { get; private set; }
        public virtual int RobotCount { get { return 2; } }
        public virtual int CompetitionId { get { return 1; } }


        public Competitions()
        {
            GameTimeLimit = 90;
            NetworkTimeLimit = 1;
            AvailableBots = new Dictionary<string, Type>();
        }

        public T GetSensorsData<T>(int robotId) where T : class, ISensorsData
        {
            return Robots[robotId].GetSensorsData<T>();
        }

        public void ApplyCommand(Command command)
        {
            Robots[command.RobotId].ProcessCommand(command);
        }


        public void Initialize(IEngine engine, RobotSettings[] robotSettings)
        {
            Engine = engine;
            Settings = ParseSettings(HelloPackage);
            Robots = robotSettings.Select(x => x.IsBot ? CreateBot(x.Number) : CreateRobot(x.Number)).ToList();
            Score = new ScoreCollection(RobotCount);
            Engine.Initialize(Settings);
            Robots.ForEach(x => x.Init());
        }

        public void MakeCycle(double time, bool realtime)
        {
            Engine.RunEngine(time, realtime);
            if (CycleFinished != null)
                CycleFinished(this, EventArgs.Empty);
        }

        public event EventHandler CycleFinished;

        public void ProcessOneParticipant(bool realtime, Participant participant)
        {
            double time = GameTimeLimit;
            while (true)
            {
                var command = participant.MakeTurn();
                Robots[command.RobotId].ProcessCommand( command);
                MakeCycle(Math.Min(time, command.Time), realtime);
                time -= command.Time;
                if (time < 0) break;
            }
        }

//        Command MakeTurn(Participant participant)
//        {
//            return participant.MakeTurn();
//        }

        Tuple<Command,Exception> MakeTurn(Participant participant)
        {
            return new Tuple<Command, Exception>(participant.MakeTurn(), null);
        }

        public void ProcessParticipants(bool realTime, int operationalMilliseconds, params Participant[] participants)
        {
            double time = GameTimeLimit;
            foreach(var e in participants) 
            {
                e.Active=true;
                e.OperationalMilliseconds=0;
                e.ExitReason= ExitReason.No;
                e.WaitForNextCommandTime = 0;
            }

            while (true)
            {
                var parts = participants.Where(z => z.Active && z.WaitForNextCommandTime <= 0);
//                foreach (var participant in parts)
//                {
//                    participant.WaitForNextCommandTime = 1;
//                    Participant participant1 = participant;
//                    Task.Factory.StartNew(() =>
//                    {
//                        var timeNow = DateTime.Now;
//                        var command = MakeTurn(participant1);
//                        command.RobotId = participant.ControlledRobot;
//                        World.Robots[participant.ControlledRobot].ProcessCommand(command);
//                        var timeForSleep = command.Time - (DateTime.Now - timeNow).TotalMilliseconds;
//                        if (timeForSleep > 0)
//                            Thread.Sleep((int)timeForSleep);
//                        participant.WaitForNextCommandTime = 0;
//                    });
//                }

                foreach (var p in parts)
                {
                    var spentMilliseconds = p.OperationalMilliseconds;
                    var @delegate = new Func<Participant, Tuple<Command, Exception>>(MakeTurn);

                    //асинхронно запускаем операцию и проверяем, что не вылезли за лимиты
                    var async = @delegate.BeginInvoke(p, null, null);
                    while (spentMilliseconds < operationalMilliseconds)
                    {
                        if (async.IsCompleted) break;
                        Thread.Sleep(1);
                        spentMilliseconds++;
                        Console.Write(spentMilliseconds+"\r");
                    }
                    Tuple<Command, Exception> result = new Tuple<Command, Exception>(null, null);
                    if (spentMilliseconds<operationalMilliseconds)
                        result = @delegate.EndInvoke(async);
                    
                    p.OperationalMilliseconds = spentMilliseconds;

                    //Проверяем ошибки и таймлимиты
                    if (spentMilliseconds >= operationalMilliseconds)
                        p.Exit(ExitReason.OperationalTimeLimit, GameTimeLimit - time, null);
                    else if (result.Item2 != null) //выкинут Exception
                        p.Exit(ExitReason.FormatException, GameTimeLimit - time, result.Item2);

                    if (!p.Active) continue;

                    //применяем полученную команду
                    var cmd=result.Item1;
                    cmd.RobotId = p.ControlledRobot;
                    Robots[p.ControlledRobot].ProcessCommand(cmd);
                    p.WaitForNextCommandTime = cmd.Time;
                }
                var minTime = Math.Min(time, participants.Min(z => z.WaitForNextCommandTime));
                if (minTime == 0 || double.IsInfinity(minTime)) break;
                MakeCycle(minTime, realTime);
                foreach (var p in participants)
                {
                    p.WaitForNextCommandTime -= minTime;
                    Console.WriteLine(p.WaitForNextCommandTime);                    
                }

                time -= minTime;
                if (time <= 0) break;
            }
        }
        
        public bool BotIsAvailable(string name)
        {
            if (name == "None") return true;
            if (AvailableBots.ContainsKey(name)) return true;
            return false;
        }

        public Bot CreateBot(string name, int controlledBot)
        {
            if (!BotIsAvailable(name)) throw new Exception("Bot not found");
            if (name == "None") return new EmptyBot
                    {
                        ControlledRobot = controlledBot
                    };
            var tp = AvailableBots[name];
            var ctor = tp.GetConstructor(new Type[] { });
            var bot = ctor.Invoke(new object[]{}) as Bot;
            bot.ControlledRobot = controlledBot;
            bot.Initialize(this);
            return bot;
        }

        public string SendPostReplay(string key, int robotNumber)
        {
            try
            {
                using (var wb = new WebClient())
                {
                    var data = new NameValueCollection();
                    data["key"] = key;
                    data["competitions"] = CompetitionId.ToString();
                    data["robotNumber"] = robotNumber.ToString();
                    data["results"] = "[" +
                                      string.Join(",",
                                                  Enumerable.Range(0, RobotCount)
                                                            .Select(
                                                                a =>
                                                                "{\"num\":\"" + a + "\", \"score\": \"" +   
                                                                Score.GetFullSumForRobot(a) + "\"}")
                                                                .ToArray()
                                                                ) + "]";
                    var replay = Engine.GetReplay();
                    data["log"] = replay;
                    wb.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.66 Safari/537.36");
                    wb.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    var res = wb.UploadValues("http://air-labs.ru/index.php/match/save", "POST", data);
                    var str = Encoding.UTF8.GetString(res);
                    return str; 
                }
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
        }
    }
}
