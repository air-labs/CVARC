using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;

namespace CVARC.V2.SimpleMovement
{
    public class SquareWalkingBot : Controller<SimpleMovementCommand>
    {
        int turn = -1;

        double distance;

        public SquareWalkingBot(double distance)
        {
            this.distance = distance;
        }

        override public SimpleMovementCommand GetCommand()
        {
            turn = (turn + 1) % 2;
            if (turn == 0) return SimpleMovementCommand.Move(distance, 1);
            else return SimpleMovementCommand.Rotate(Angle.Pi / 2, 1);
            
        }

    }
}
