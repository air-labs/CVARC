using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;

namespace CVARC.V2.SimpleMovement
{
    public class RandomWalkingBot : Controller<SimpleMovementCommand>
    {
        int turn = -1;
        Random rnd = new Random();

        double distance;

        public RandomWalkingBot(double distance)
        {
            this.distance = distance;
        }

        override public SimpleMovementCommand GetCommand()
        {
            turn = (turn + 1) % 2;
            if (turn==0) return SimpleMovementCommand.Move(distance, 1);
            else return SimpleMovementCommand.Rotate(Angle.FromGrad(rnd.Next(360)-180), 1 );
            
        }

    }
}
