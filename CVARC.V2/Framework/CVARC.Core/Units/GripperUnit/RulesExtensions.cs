using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CVARC.V2.Units
{
	public static partial class RulesExtensions
	{
		public static TCommand Grip<TCommand>(this IGripperRules<TCommand> factory)
			where TCommand : IGripperCommand, new()
		{
			return new TCommand { GripperCommand = GripperAction.Grip };
		}

		public static TCommand Release<TCommand>(this IGripperRules<TCommand> factory)
			where TCommand : IGripperCommand, new()
		{
			return new TCommand { GripperCommand = GripperAction.Release };
		}

		public static void AddGripKeys<TCommand>(this IGripperRules<TCommand> rules, KeyboardControllerPool<TCommand> pool)
			where TCommand : IGripperCommand, new()
		{
			pool.Add(Keys.R, TwoPlayersId.Left, () => new TCommand { GripperCommand = GripperAction.Grip });
			pool.Add(Keys.F, TwoPlayersId.Left, () => new TCommand { GripperCommand = GripperAction.Release });

			pool.Add(Keys.P, TwoPlayersId.Right, () => new TCommand { GripperCommand = GripperAction.Grip });
			pool.Add(Keys.OemSemicolon, TwoPlayersId.Right, () => new TCommand { GripperCommand = GripperAction.Release });
		}

	}
}
