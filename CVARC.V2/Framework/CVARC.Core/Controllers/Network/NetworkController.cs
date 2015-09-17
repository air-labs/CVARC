using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AIRLab;

namespace CVARC.V2
{

    public class NetworkController<TCommand> : INetworkController
        where TCommand : ICommand
    {
        IMessagingClient client;

        double OperationalTime;
        double OperationalTimeLimit;

        public void InitializeClient(IMessagingClient client)
        {
            this.client = client;
			client.EnableDebug = true;
        }

        public void Initialize(IActor controllableActor)
        {
            OperationalTimeLimit = controllableActor.World.Configuration.Settings.OperationalTimeLimit;
        }

        object sensorData;
        bool active = true;

        Tuple<ICommand, Exception> GetCommandInternally(Type commandType)
        {
            try
            {
                client.Write(sensorData);
                var command = (ICommand)client.Read(commandType);
			    return new Tuple<ICommand,Exception>(command,null);
            }
            catch (Exception e)
            {
                return new Tuple<ICommand,Exception>(null,e);
            }
        }

        public ICommand GetCommand()
        {
            if (!active) return null;

            var @delegate = new Func<Type, Tuple<ICommand, Exception>>(GetCommandInternally);

            var async = @delegate.BeginInvoke(typeof(TCommand), null, null);

			OperationalTimeLimit = 0.1; //TODO: убрать рак

            while (OperationalTime <OperationalTimeLimit)
            {
                if (async.IsCompleted) break;
                OperationalTime += 0.001;
                Thread.Sleep(1);
            }

            if (OperationalTime < OperationalTimeLimit)
            {
                var result = @delegate.EndInvoke(async);
                if (result.Item2 != null) return null;
                
                Debugger.Log(DebuggerMessageType.Workflow,"Command accepted in controller");
                return result.Item1;
            }

            Thread.Sleep(100);//without this sleep, if computer performs badly and units contain multiple triggers, the server will be stopped before test client receives data, hence client will throw exception.
       
			client.Close();
			Debugger.Log(DebuggerMessageType.Error, "Can't get command");
            return null;

        }

        public void SendSensorData(object sensorData)
        {
            this.sensorData = sensorData;   
        }
    }
}
