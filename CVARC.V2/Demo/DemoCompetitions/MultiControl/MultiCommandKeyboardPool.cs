using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace Demo
{
    public class MultiCommandKeyboardPool : KeyboardControllerPool<object>
    {
        public override void Initialize(IWorld world, IKeyboard keyboard)
        {
            base.Initialize(world, keyboard);
            var dt = 0.1;
            Add(Keys.W, "Left", () => SimpleMovementCommand.Move(50, dt));
            Add(Keys.S, "Left", () => SimpleMovementCommand.Move(-50, dt));
            Add(Keys.A, "Left", () => SimpleMovementCommand.Rotate(Angle.Pi, dt));
            Add(Keys.D, "Left", () => SimpleMovementCommand.Rotate(-Angle.Pi, dt));

            Add(Keys.I, "Left", () => new Displacement { DY = 50 });
            Add(Keys.K, "Left", () => new Displacement { DY = -50 });
            Add(Keys.J, "Left", () => new Displacement { DX = -50 });
            Add(Keys.L, "Left", () => new Displacement { DX = 50 });

            StopCommandFactory = () => SimpleMovementCommand.Move(0, dt);
        }
    }
}
