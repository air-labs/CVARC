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
            if (turn==0) return new SimpleMovementCommand { LinearVelocity = distance, Duration = 1 };
            else return new SimpleMovementCommand { AngularVelocity =- Angle.Pi/2, Duration = 1 };
            
        }

    }
}
