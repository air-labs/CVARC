using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;

namespace CVARC.V2.SimpleMovement
{
    public class SquareWalkingBot : IController<SimpleMovementCommand>
    {
        int turn = 0;

        public SimpleMovementCommand GetCommand()
        {
            if (turn==0) return new SimpleMovementCommand { LinearVelocity = 100, Duration = 1 };
            else if (turn==1) return new SimpleMovementCommand { Command="Action" };
            else return new SimpleMovementCommand { AngularVelocity = Angle.Pi/2, Duration = 1 };
            turn = (turn + 1) % 3;
        }

        public void Initialize(IWorld world)
        {
           
        }

        public int ControllerNumber
        {
            get;
            set; 
        }
    }
}
