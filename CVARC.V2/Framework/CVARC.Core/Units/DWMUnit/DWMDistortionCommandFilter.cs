using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public class DWMDistortionCommandFilter : ICommandFilter
	{
		public double Multiplier = 1;

		public IEnumerable<ICommand> Preprocess(IActor actor, ICommand command)
		{
			if (!(command is IDWMCommand))
			{
				yield return command;
				yield break;
			}
			var cmd = command as IDWMCommand;
			if (cmd.DifWheelMovement == null)
			{
				yield return command;
				yield break;
			}



			cmd.DifWheelMovement = new DifWheelMovement
			{
				LeftRotatingVelocity=cmd.DifWheelMovement.LeftRotatingVelocity*Multiplier,
				RightRotatingVelocity = cmd.DifWheelMovement.RightRotatingVelocity*Multiplier,
				Duration=cmd.DifWheelMovement.Duration,
			
			};
			yield return cmd as ICommand;

		}

		public void Initialize(IActor actor)
		{
			
		}
	}
}
