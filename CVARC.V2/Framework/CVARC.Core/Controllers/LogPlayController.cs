using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class LogPlayController : IController
    {
        List<object> commands;
        int pointeer = 0;

        public LogPlayController(IEnumerable<object> commands)
        {
            this.commands = commands.ToList();
        }

        public void Initialize(IActor controllableActor)
        {
          
        }

        public object GetCommand()
        {
            var command=commands[pointeer];
            pointeer++;
            return command;
        }

        public void SendSensorData(object sensorData)
        {
           
        }
    }
}