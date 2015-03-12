using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class ControlTrigger : RenewableTrigger
    {
        IController controller;
        IActor controllable;
        ICommandPreprocessor preprocessor;
        IEnumerator<ICommand> currentBuffer;

        public ControlTrigger(IController controller, IActor controllable, ICommandPreprocessor preprocessor)
        {
            this.controller = controller;
            this.controllable = controllable;
            this.preprocessor = preprocessor;
        }


        bool FillBuffer()
        {
            var sensorData = controllable.GetSensorData();
            controller.SendSensorData(sensorData);
            var command = controller.GetCommand();
			if (command == null) return false;
			controllable.World.Logger.AccountCommand(controllable.ControllerId, command);
            var processedCommands = preprocessor.Preprocess(command);
            currentBuffer = processedCommands.GetEnumerator();
            if (!currentBuffer.MoveNext())
            {
                throw new Exception("The preprocessor has returned an empty set of commands. Unable to processd");
            }
            return true;
        }

        public override void Act(out double nextTime)
        {
            if (currentBuffer == null)
                if (!FillBuffer())
                {
                    nextTime = double.PositiveInfinity;
                    return;
                }
            var currentCommand = currentBuffer.Current;
            double duration;
            controllable.ExecuteCommand(currentCommand, out duration);
            nextTime = base.ThisCall +  duration;
            if (!currentBuffer.MoveNext())
                currentBuffer = null;
        }
    }
}
