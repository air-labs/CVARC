using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AIRLab;

namespace CVARC.V2
{

    public class NetworkController<TCommand> : INetworkController
    {
        CvarcTcpClient client;

        public double OperationalTimeLimit { get; set; }
        double OperationalTime;

        public void InitializeClient(CvarcTcpClient client)
        {
            this.client = client;
        }

        public void Initialize(IActor controllableActor)
        {
            
        }

        object sensorData;
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

        public object GetCommand()
        {
            if (!active) return null;

            var @delegate = new Func<Type, Tuple<ICommand, Exception>>(GetCommandInternally);

            var async = @delegate.BeginInvoke(typeof(TCommand), null, null);

            while (OperationalTime < OperationalTimeLimit)
            {
                if (async.IsCompleted) break;
                OperationalTime += 0.001;
                Thread.Sleep(1);
            }

            if (OperationalTime < OperationalTimeLimit)
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
