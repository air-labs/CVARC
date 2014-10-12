using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class LogPlayController : IController
    {
        List<ICommand> commands;
        int pointeer = 0;

        public LogPlayController(IEnumerable<ICommand> commands)
        {
            this.commands = commands.ToList();
        }

        public void Initialize(IActor controllableActor)
        {
          
        }

        public ICommand GetCommand()
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