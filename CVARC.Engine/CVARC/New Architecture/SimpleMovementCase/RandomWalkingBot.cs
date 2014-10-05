using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;

namespace CVARC.V2.SimpleMovement
{
    public class RandomWalkingBot : IController<SimpleMovementCommand>
    {
        int turn = -1;
        Random rnd = new Random();

        double distance;

        public RandomWalkingBot(double distance)
        {
            this.distance = distance;
        }

        public SimpleMovementCommand GetCommand()
        {
            turn = (turn + 1) % 2;
            if (turn==0) return new SimpleMovementCommand { LinearVelocity = distance, Duration = 1 };
            else return new SimpleMovementCommand { AngularVelocity = Angle.FromGrad(rnd.Next(360)-180), Duration = 1 };
            
        }

        public void Initialize(IWorld world)
        {
           
        }
    }
}
