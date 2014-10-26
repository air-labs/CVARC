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
            Add(Keys.Q, TwoPlayersId.Left, () => SimpleMovementCommand.Action("Grip"));
            Add(Keys.E, TwoPlayersId.Left, () => SimpleMovementCommand.Action("Release"));

            Add(Keys.U, TwoPlayersId.Right, () => SimpleMovementCommand.Action("Grip"));
            Add(Keys.O, TwoPlayersId.Right, () => SimpleMovementCommand.Action("Release"));
        }
    }
}
