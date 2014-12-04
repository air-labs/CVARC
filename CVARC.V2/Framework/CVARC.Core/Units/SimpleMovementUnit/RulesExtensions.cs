using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CVARC.V2.Units
{
	public static partial class RulesExtensions
	{
		

		public static TCommand Stand<TCommand>(this ISimpleMovementRules<TCommand> factory, double time)
			where TCommand : ISimpleMovementCommand, new()
		{
			return new TCommand { SimpleMovement = SimpleMovement.Stand(time) };
		}

		public static TCommand Move<TCommand>(this ISimpleMovementRules<TCommand> factory, double length)
			where TCommand : ISimpleMovementCommand, new()
		{
			return new TCommand { SimpleMovement = SimpleMovement.MoveWithVelocity(length, factory.LinearVelocityLimit) };
		}

		public static TCommand Rotate<TCommand>(this ISimpleMovementRules<TCommand> factory, Angle angle)
			where TCommand : ISimpleMovementCommand, new()
		{
			return new TCommand { SimpleMovement = SimpleMovement.RotateWithVelocity(angle, factory.AngularVelocityLimit) };
		}

		public static void AddSimpleMovementKeys<TCommand>(this ISimpleMovementRules<TCommand> factory, KeyboardControllerPool<TCommand> pool)
			where TCommand :  ISimpleMovementCommand, new()
		{
			var dt = 0.1;
			pool.Add(Keys.W, TwoPlayersId.Left, () => new TCommand { SimpleMovement = SimpleMovement.Move(factory.LinearVelocityLimit, dt) });
			pool.Add(Keys.S, TwoPlayersId.Left, () => new TCommand { SimpleMovement = SimpleMovement.Move(-factory.LinearVelocityLimit, dt) });
			pool.Add(Keys.I, TwoPlayersId.Right, () => new TCommand { SimpleMovement = SimpleMovement.Move(factory.LinearVelocityLimit, dt) });
			pool.Add(Keys.K, TwoPlayersId.Right, () => new TCommand { SimpleMovement = SimpleMovement.Move(-factory.LinearVelocityLimit, dt) });

			pool.Add(Keys.A, TwoPlayersId.Left, () => new TCommand { SimpleMovement = SimpleMovement.Rotate(factory.AngularVelocityLimit, dt) });
			pool.Add(Keys.D, TwoPlayersId.Left, () => new TCommand { SimpleMovement = SimpleMovement.Rotate(-factory.AngularVelocityLimit, dt) });
			pool.Add(Keys.J, TwoPlayersId.Right, () => new TCommand { SimpleMovement = SimpleMovement.Rotate(factory.AngularVelocityLimit, dt) });
			pool.Add(Keys.L, TwoPlayersId.Right, () => new TCommand { SimpleMovement = SimpleMovement.Rotate(-factory.AngularVelocityLimit, dt) });
		
		}
	}
}
