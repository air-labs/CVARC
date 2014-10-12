using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class NetworkController : IController
    {
        GroboTcpClient client;
        private static readonly ISerializer Serializer = new JsonSerializer();
    
        public NetworkController(GroboTcpClient client)
        {
            this.client = client;
        }

        public Configuration ReadConfiguration()
        {
            return (Configuration)Serializer.Deserialize(typeof(Configuration), client.ReadToEnd());
        }

        public void Initialize(IActor controllableActor)
        {
           
        }

        object sensorData;


        public ICommand GetCommand(Type commandType)
        {
            client.Send(Serializer.Serialize(sensorData));
            var command = (ICommand)Serializer.Deserialize(commandType, client.ReadToEnd());
            return command;

            // этот код проверял исключения и превышение операционного лимита. его нужно вернуть!
               //var spentMilliseconds = p.OperationalMilliseconds;
               //     var @delegate = new Func<Participant, Tuple<Command, Exception>>(MakeTurn);

               //     //асинхронно запускаем операцию и проверяем, что не вылезли за лимиты
               //     var async = @delegate.BeginInvoke(p, null, null);
               //     while (spentMilliseconds < operationalMilliseconds)
               //     {
               //         if (async.IsCompleted) break;
               //         Thread.Sleep(1);
               //         spentMilliseconds++;
               //         Console.Write(spentMilliseconds+"\r");
               //     }
               //     Tuple<Command, Exception> result = new Tuple<Command, Exception>(null, null);
               //     if (spentMilliseconds<operationalMilliseconds)
               //         result = @delegate.EndInvoke(async);
                    
               //     p.OperationalMilliseconds = spentMilliseconds;

               //     //Проверяем ошибки и таймлимиты
               //     if (spentMilliseconds >= operationalMilliseconds)
               //         p.Exit(ExitReason.OperationalTimeLimit, GameTimeLimit - time, null);
               //     else if (result.Item2 != null) //выкинут Exception
               //         p.Exit(ExitReason.FormatException, GameTimeLimit - time, result.Item2);

               //     if (!p.Active) continue;

               //     //применяем полученную команду
               //     var cmd=result.Item1;
               //     cmd.RobotId = p.ControlledRobot;
               //     Robots[p.ControlledRobot].ProcessCommand(cmd);
               //     p.WaitForNextCommandTime = cmd.Time;
        }



        public void SendSensorData(object sensorData)
        {
            this.sensorData = sensorData;   
        }
    }
}
