using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace RepairTheStarship
{
    public class RTSKeyboardControllerPool : SimpleMovementTwoPlayersKeyboardControllerPool
    {
        public override void Initialize(IWorld world, IKeyboard keyboard)
        {
            base.Initialize(world, keyboard);
            Add(Keys.Q, TwoPlayersId.Left, () => new SimpleMovementCommand { Command = "Grip" });
            Add(Keys.E, TwoPlayersId.Left, () => new SimpleMovementCommand { Command = "Release" });

            Add(Keys.U, TwoPlayersId.Right, () => new SimpleMovementCommand { Command = "Grip" });
            Add(Keys.O, TwoPlayersId.Right, () => new SimpleMovementCommand { Command = "Release" });
        }
    }
}
