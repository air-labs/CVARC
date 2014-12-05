using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public class Bot<TCommand> : Controller<TCommand>
        where TCommand : ICommand
	{
		Func<int, TCommand> CommandProvider;
		int turnNumber = 0;


		public Bot(Func<int, TCommand> CommandProvider)
		{
			this.CommandProvider = CommandProvider;
		}

		public override TCommand GetCommand()
		{
			var result = CommandProvider(turnNumber);
			turnNumber++;
			return result;
		}
	}
}
