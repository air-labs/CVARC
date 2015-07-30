using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CVARC.V2
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

        public static TCommand MoveWithVelocityForTime<TCommand>(this ISimpleMovementRules<TCommand> factory, double velocity, double time)
            where TCommand : ISimpleMovementCommand, new()
        {
            return new TCommand { SimpleMovement = SimpleMovement.Move(velocity, time) };
        }

        public static TCommand MovePathWithVelocity<TCommand>(this ISimpleMovementRules<TCommand> factory, double path, double velocity)
            where TCommand : ISimpleMovementCommand, new()
        {
            return new TCommand { SimpleMovement = SimpleMovement.MoveWithVelocity(path, velocity) };
        }

		public static TCommand Rotate<TCommand>(this ISimpleMovementRules<TCommand> factory, Angle angle)
			where TCommand : ISimpleMovementCommand, new()
		{
			return new TCommand { SimpleMovement = SimpleMovement.RotateWithVelocity(angle, factory.AngularVelocityLimit) };
		}

        public static TCommand RotateWithVelocityForTime<TCommand>(this ISimpleMovementRules<TCommand> factory, Angle velocity, double time)
            where TCommand : ISimpleMovementCommand, new()
        {
            return new TCommand { SimpleMovement = SimpleMovement.Rotate(velocity, time) };
        }

        public static TCommand RotateAngleWithVelocity<TCommand>(this ISimpleMovementRules<TCommand> factory, Angle angle, Angle velocity)
            where TCommand : ISimpleMovementCommand, new()
        {
            return new TCommand { SimpleMovement = SimpleMovement.RotateWithVelocity(angle, velocity) };
        }

		public static void AddSimpleMovementKeys<TCommand>(this ISimpleMovementRules<TCommand> factory, KeyboardController<TCommand> pool, string controllerId)
			where TCommand : ISimpleMovementCommand, new()
		{
			var dt = 0.1;

			if (controllerId == TwoPlayersId.Left)
			{
				pool.Add(Keys.W, () => new TCommand { SimpleMovement = SimpleMovement.Move(factory.LinearVelocityLimit, dt) });
				pool.Add(Keys.S, () => new TCommand { SimpleMovement = SimpleMovement.Move(-factory.LinearVelocityLimit, dt) });
                pool.Add(Keys.A, () => new TCommand { SimpleMovement = SimpleMovement.Rotate(factory.AngularVelocityLimit, dt) });
                pool.Add(Keys.D, () => new TCommand { SimpleMovement = SimpleMovement.Rotate(-factory.AngularVelocityLimit, dt) });
            }

			if (controllerId == TwoPlayersId.Right)
			{
				pool.Add(Keys.I, () => new TCommand { SimpleMovement = SimpleMovement.Move(factory.LinearVelocityLimit, dt) });
                pool.Add(Keys.K, () => new TCommand { SimpleMovement = SimpleMovement.Move(-factory.LinearVelocityLimit, dt) });
                pool.Add(Keys.J, () => new TCommand { SimpleMovement = SimpleMovement.Rotate(factory.AngularVelocityLimit, dt) });
                pool.Add(Keys.L, () => new TCommand { SimpleMovement = SimpleMovement.Rotate(-factory.AngularVelocityLimit, dt) });
            }
		}

		public static Bot<TCommand> CreateStandingBot<TCommand>(this ISimpleMovementRules<TCommand> factory)
			where TCommand : ISimpleMovementCommand, new()
		{
			return new Bot<TCommand>(turn => factory.Stand(1));
		}

		public static Bot<TCommand> CreateSquareWalkingBot<TCommand>(this ISimpleMovementRules<TCommand> factory, int distance)
			where TCommand : ISimpleMovementCommand, new()
		{
			return new Bot<TCommand>(turn =>
				turn % 2 == 0 ?
					factory.Move(distance) :
					factory.Rotate(Angle.HalfPi)
					);
		}

		public static Bot<TCommand> CreateRandomWalkingBot<TCommand>(this ISimpleMovementRules<TCommand> factory, int distance)
			where TCommand : ISimpleMovementCommand, new()
		{
			var random = new Random();
			return new Bot<TCommand>(turn =>
				turn % 2 == 0 ?
					factory.Move(distance) :
					factory.Rotate(Angle.FromGrad(random.NextDouble() * 360))
					);
		}
	}
}
