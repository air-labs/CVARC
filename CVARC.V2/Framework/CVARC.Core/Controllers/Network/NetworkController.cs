using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AIRLab;

namespace CVARC.V2
{

    public class NetworkController : IController
    {
        CvarcTcpClient client;

        public NetworkController(CvarcTcpClient client, double operationalLimitInSeconds)
        {
            this.client = client;
            if (double.IsPositiveInfinity(operationalLimitInSeconds))
                operationalLimit = int.MaxValue;
            else
                operationalLimit = (int)(operationalLimitInSeconds*1000);
        }

        public Configuration ReadConfiguration()
        {
            return client.ReadObject<Configuration>();    
        }

        public void Initialize(IActor controllableActor)
        {
           
        }

        object sensorData;
        int operationalTime = 0;
        int operationalLimit;
        bool active = true;

        Tuple<ICommand, Exception> GetCommandInternally(Type commandType)
        {
            try
            {
                client.SerializeAndSend(sensorData);
                var command = (ICommand)client.ReadObject(commandType);
                return new Tuple<ICommand,Exception>(command,null);
            }
            catch (Exception e)
            {
                return new Tuple<ICommand,Exception>(null,e);
            }
        }

        public ICommand GetCommand(Type commandType)
        {
            if (!active) return null;

            var @delegate = new Func<Type, Tuple<ICommand, Exception>>(GetCommandInternally);

            var async = @delegate.BeginInvoke(commandType, null, null);

            while (operationalTime < operationalLimit)
            {
                if (async.IsCompleted) break;
                operationalTime++;
                Thread.Sleep(1);
            }

            if (operationalTime < operationalLimit)
            {
                var result = @delegate.EndInvoke(async);
                if (result.Item2 != null) return null;
                return result.Item1;
            }
            return null;

        }



        public void SendSensorData(object sensorData)
        {
            this.sensorData = sensorData;   
        }
    }
}
