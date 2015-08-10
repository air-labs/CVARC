using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class ControlTrigger : RenewableTrigger
    {
        IController controller;
        IActor controllable;
        CommandFilterSet filterSet;

        public ControlTrigger(IController controller, IActor controllable, CommandFilterSet filterSet)
        {
            this.controller = controller;
            this.controllable = controllable;
            this.filterSet = filterSet;
        }


        bool FillBuffer()
        {
            var sensorData = controllable.GetSensorData();
            controller.SendSensorData(sensorData);
            var command = controller.GetCommand();
			if (command == null) return false;
            Debugger.Log(DebuggerMessageType.Workflow, "Command accepted in ControlTrigger");
			controllable.World.Logger.AccountCommand(controllable.ControllerId, command);

            filterSet.ProcessCommand(controllable, command);
            if (!filterSet.CommandAvailable)
            {
                throw new Exception("The preprocessor has returned an empty set of commands. Unable to processd");
            }
            return true;
        }

        public override void Act(out double nextTime)
        {
            if (!filterSet.CommandAvailable)
                if (!FillBuffer())
                {
                    nextTime = double.PositiveInfinity;
                    return;
                }
            var currentCommand = filterSet.GetNextCommand();
            double duration;
            Debugger.Log(DebuggerMessageType.Workflow, "Command goes to robot");
            controllable.ExecuteCommand(currentCommand, out duration);
            nextTime = base.ThisCall +  duration;
        }
    }
}
