using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using AIRLab.Mathematics;
using CVARC.Basic.Controllers;
using CVARC.Basic.Core.Participants;
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
        public virtual double LinearVelocityLimit { get { return 50; } }
        public virtual double AngularVelocityLimit { get { return 90; } }
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
        private bool needSaveReplay;

        public double InternalTime = 0;

		public event Action Exited;

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


        public virtual void Initialize(IEngine engine, RobotSettings[] robotSettings)
        {
            Engine = engine;
            Settings = ParseSettings(HelloPackage);
            Robots = robotSettings.Select(x => x.IsBot ? CreateBot(x.Number) : CreateRobot(x.Number)).OrderBy(x => x.Number).ToList();
            Score = new ScoreCollection(RobotCount);
            Engine.Initialize(Settings);
            Robots.ForEach(x => x.Init());
        }

        public void MakeCycle(double time, bool realtime)
        {
            Engine.RunEngine(time, realtime);
            InternalTime = time;
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
            var command = participant.MakeTurn();
            if (command.Action != CommandAction.None)
                command.Time = 1;
            if (command.LinearVelocity != 0 && command.AngularVelocity.Grad != 0)
                command.LinearVelocity = 0;
            if (command.Time < 0)
                command.Time = 0;
            if (Math.Abs(command.LinearVelocity) > LinearVelocityLimit)
               command.LinearVelocity = Math.Sign(command.LinearVelocity) * LinearVelocityLimit;
            if (command.AngularVelocity.Grad > AngularVelocityLimit)
                command.AngularVelocity = Angle.FromGrad(Math.Sign(command.AngularVelocity.Grad) * AngularVelocityLimit);
            return new Tuple<Command, Exception>(command, null);
        }

        public void ProcessParticipants(bool realTime, int operationalMilliseconds, bool saveReplay = false, params Participant[] participants)
        {
            needSaveReplay = saveReplay;
            double time = GameTimeLimit;
            foreach(var e in participants) 
            {
                e.Active=true;
                e.OperationalMilliseconds=0;
                e.ExitReason= ExitReason.No;
                e.WaitForNextCommandTime = 0;
            }


			bool clientExited = false;
			

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

					if (p is NetworkParticipant && !p.Active)
						clientExited = true;

                    if (!p.Active) continue;


                    //применяем полученную команду
                    var cmd=result.Item1;
                    cmd.RobotId = p.ControlledRobot;
                    Robots[p.ControlledRobot].ProcessCommand(cmd);
                    p.WaitForNextCommandTime = cmd.Time;
					if (cmd.Action == CommandAction.WaitForExit)
						clientExited = true;
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
				if (clientExited) break;
            }
			if (needSaveReplay)
                SaveReplay(time);			
            if (Exited != null) 
                Exited();
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

        private void SaveReplay(double gameTime)
        {
            var replay = Engine.GetReplay();
            var scores = Score.GetFullSumForRobot(0) + ":" + Score.GetFullSumForRobot(1);
            var lines = new[]
                {
                    "C#",
                    scores,
                    (GameTimeLimit - gameTime).ToString(),
                    replay
                };
            const string path = "RawReplays\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllLines(path + Guid.NewGuid(), lines);
        }
    }
}
