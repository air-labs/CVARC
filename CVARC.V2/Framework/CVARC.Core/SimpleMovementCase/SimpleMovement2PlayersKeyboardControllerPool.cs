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
            var v = Compatibility.Check<ISimpleMovementWorld>(this, world);
            double time = 0.1;
            Add(Keys.W, TwoPlayersId.Left, () => SimpleMovementCommand.Move(v.CommandHelper.LinearVelocityLimit, time ));
            Add(Keys.S, TwoPlayersId.Left, () => SimpleMovementCommand.Move (-v.CommandHelper.LinearVelocityLimit,  time ));
            Add(Keys.I, TwoPlayersId.Right, () => SimpleMovementCommand.Move (v.CommandHelper.LinearVelocityLimit,  time ));
            Add(Keys.K, TwoPlayersId.Right, () => SimpleMovementCommand.Move (-v.CommandHelper.LinearVelocityLimit,  time ));

            Add(Keys.A, TwoPlayersId.Left, () => SimpleMovementCommand.Rotate(v.CommandHelper.AngularVelocityLimit,  time ));
            Add(Keys.D, TwoPlayersId.Left, () => SimpleMovementCommand.Rotate(-v.CommandHelper.AngularVelocityLimit,  time ));
            Add(Keys.J, TwoPlayersId.Right, () => SimpleMovementCommand.Rotate(v.CommandHelper.AngularVelocityLimit,  time ));
            Add(Keys.L, TwoPlayersId.Right, () => SimpleMovementCommand.Rotate(-v.CommandHelper.AngularVelocityLimit,  time ));

            StopCommandFactory = ()=>SimpleMovementCommand.Move(0,time);
        }

    }
}
