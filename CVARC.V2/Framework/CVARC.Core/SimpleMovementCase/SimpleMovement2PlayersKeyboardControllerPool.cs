using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AIRLab.Mathematics;

namespace CVARC.V2.SimpleMovement
{
    public class SimpleMovementTwoPlayersKeyboardControllerPool : KeyboardControllerPool<SimpleMovementCommand>
    {
        public override void Initialize(IWorld world, IKeyboard keyboard)
        {
            base.Initialize(world, keyboard);
            var v = world as ISimpleMovementWorld;
            double time = 0.1;
            double linear = 50;
            Angle angular=Angle.Pi;
            Add(Keys.W, TwoPlayersId.Left, () => new SimpleMovementCommand { LinearVelocity = v.CommandHelper.LinearVelocityLimit, Duration = time });
            Add(Keys.S, TwoPlayersId.Left, () => new SimpleMovementCommand { LinearVelocity = -v.CommandHelper.LinearVelocityLimit, Duration = time });
            Add(Keys.I, TwoPlayersId.Right, () => new SimpleMovementCommand { LinearVelocity = v.CommandHelper.LinearVelocityLimit, Duration = time });
            Add(Keys.K, TwoPlayersId.Right, () => new SimpleMovementCommand { LinearVelocity = -v.CommandHelper.LinearVelocityLimit, Duration = time });

            Add(Keys.A, TwoPlayersId.Left, () => new SimpleMovementCommand { AngularVelocity = v.CommandHelper.AngularVelocityLimit, Duration = time });
            Add(Keys.D, TwoPlayersId.Left, () => new SimpleMovementCommand { AngularVelocity = -v.CommandHelper.AngularVelocityLimit, Duration = time });
            Add(Keys.J, TwoPlayersId.Right, () => new SimpleMovementCommand { AngularVelocity = v.CommandHelper.AngularVelocityLimit, Duration = time });
            Add(Keys.L, TwoPlayersId.Right, () => new SimpleMovementCommand { AngularVelocity = -v.CommandHelper.AngularVelocityLimit, Duration = time });

            StopCommandFactory = ()=>new SimpleMovementCommand { Duration = time };
        }

    }
}
