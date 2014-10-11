using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class ControlTrigger : RenewableTrigger
    {
        IController controller;
        IControllable controllable;

        public ControlTrigger(IController controller, IControllable controllable)
        {
            this.controller = controller;
            this.controllable = controllable;
        }

        public override void Act(out double nextTime)
        {
            var sensorData = controllable.GetSensorData();
            controller.SendSensorData(sensorData);
            var command = controller.GetCommand();
            controllable.ExecuteCommand(command);
            nextTime = base.ThisCall + command.Duration;
        }
    }
}
