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
using CVARC.Core.Replay;
using CVARC.Graphics;
using CVARC.Physics;

namespace CVARC.Basic
{
    public class Competitions
    {
        public readonly World World;
        public readonly KeyboardController KeyboardController;
        public readonly NetworkController NetworkController;
        public double GameTimeLimit { get; protected set; }
        public double NetworkTimeLimit { get; protected set; }
        public virtual double LinearVelocityLimit { get { return 10; } }
        public virtual Angle AngularVelocityLimit { get { return Angle.FromGrad(30); } }
        public Body Root { get { return World.Root; } }
        public DrawerFactory DrawerFactory { get { return World.DrawerFactory; }}
        public Dictionary<string, Type> AvailableBots { get; private set; }
        public ReplayLogger Logger { get; private set; }

        public Competitions(World world, KeyboardController keyboard, NetworkController network)
        {
            World = world;
            KeyboardController = keyboard;
            NetworkController = network;
            GameTimeLimit = 90;
            NetworkTimeLimit = 1;
            AvailableBots = new Dictionary<string, Type>();
        }

        public ISensorsData GetSensorsData(int robotId)
        {
            return World.Robots[robotId].GetSensorsData();
        }

        public void ApplyCommand(Command command)
        {
            World.Robots[command.RobotId].ProcessCommand(command);
        }

        public static Competitions Load(CompetitionsSettings settings)
        {
            if (string.IsNullOrEmpty(settings.CompetitionsName) || !File.Exists(settings.CompetitionsName))
                throw new Exception(string.Format("Файл соревнований {0} не был найден. Проверьте правильность пути CompetitionsName в файле настроек: {1}.", settings.CompetitionsName, settings.SettingsFileName));

            var ass = Assembly.LoadFrom(settings.CompetitionsName);
            var competitions = ass.GetExportedTypes().SingleOrDefault(a => a.IsSubclassOf(typeof(Competitions)) && a.Name == settings.LevelName);
            if (competitions == null)
                throw new Exception(string.Format("Уровень {0} не был найден в {1}", settings.LevelName, settings.CompetitionsName));
            var ctor = competitions.GetConstructor(new Type[] {});
            return ctor.Invoke(new object[] {}) as Competitions;
        }

        public void Initialize()
        {
            World.Init();            
            PhysicalManager.InitializeEngine(PhysicalEngines.Farseer, Root);
            Logger = new ReplayLogger(Root, 0.1);
        }

        public void MakeCycle(double time, bool realtime)
        {
            double dt = 1.0 / 100;
            int span=(int)(dt*1000);
            for (double t = 0; t < time; t += dt)
            {
                Logger.LogBodies();
                foreach (var robot in World.Robots)
                    robot.SetVelocity(); 
                PhysicalManager.MakeIteration(dt, Root);
                foreach (Body body in Root)
                    body.Update(1 / 60);
                if (realtime)
                    Thread.Sleep(span);
            }
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
                World.Robots[command.RobotId].ProcessCommand( command);
                MakeCycle(Math.Min(time, command.Time), realtime);
                time -= command.Time;
                if (time < 0) break;
            }
        }

        Tuple<Command, Exception> MakeTurn(Participant participant)
        {
            try
            {
                return new Tuple<Command, Exception>(participant.MakeTurn(), null);
            }
            catch (Exception e)
            {
                return new Tuple<Command, Exception>(null, e);
            }
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
                if (!parts.Any()) break;
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
                    World.Robots[p.ControlledRobot].ProcessCommand(cmd);
                    p.WaitForNextCommandTime = cmd.Time;
                }
                var minTime = Math.Min(time, participants.Min(z => z.WaitForNextCommandTime));
                if (minTime == 0 || double.IsInfinity(minTime)) break;
                MakeCycle(minTime, realTime);
                foreach (var p in participants)
                    p.WaitForNextCommandTime -= minTime;
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
            if (name == "None") return new EmptyBot();
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
                    data["competitions"] = World.CompetitionId.ToString();
                    data["robotNumber"] = robotNumber.ToString();
                    data["results"] = "[" +
                                      string.Join(",",
                                                  Enumerable.Range(0, World.RobotCount)
                                                            .Select(
                                                                a =>
                                                                "{\"num\":\"" + a + "\", \"score\": \"" +   
                                                                World.Score.GetFullSumForRobot(a) + "\"}")) + "]";
                    var replay = ConverterToJavaScript.Convert(Logger.SerializationRoot);
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
