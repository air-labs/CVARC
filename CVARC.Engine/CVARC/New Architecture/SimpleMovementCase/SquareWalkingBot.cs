using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;

namespace CVARC.V2.SimpleMovement
{
    public class SquareWalkingBot : IController<SimpleMovementCommand>
    {
        bool turnRequired;

        public SimpleMovementCommand GetCommand()
        {
            if (turnRequired) return new SimpleMovementCommand { AngularVelocity = Angle.Pi/2, Duration = 1 };
            else return new SimpleMovementCommand { LinearVelocity = 100, Duration = 1 };
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
