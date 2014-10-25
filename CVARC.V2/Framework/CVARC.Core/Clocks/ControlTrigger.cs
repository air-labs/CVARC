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


        void FillBuffer()
        {
            var sensorData = controllable.GetSensorData();
            controller.SendSensorData(sensorData);
            var command = controller.GetCommand();
            controllable.World.Logger.AccountCommand(controllable.ControllerId, command);
            var processedCommands = preprocessor.Preprocess(command);
            currentBuffer = processedCommands.GetEnumerator();
            if (!currentBuffer.MoveNext())
            {
                throw new Exception("The preprocessor has returned an empty set of commands. Unable to processd");
            }
        }

        public override void Act(out double nextTime)
        {
            if (currentBuffer == null)
                FillBuffer();
            var currentCommand = currentBuffer.Current;
            controllable.ExecuteCommand(currentCommand);
            nextTime = base.ThisCall + currentCommand.Duration;
            if (!currentBuffer.MoveNext())
                currentBuffer = null;
        }
    }
}
