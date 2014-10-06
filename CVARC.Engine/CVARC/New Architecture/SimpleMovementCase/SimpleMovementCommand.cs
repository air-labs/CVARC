using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;

namespace CVARC.V2.SimpleMovement
{
    public class SimpleMovementCommand : ICommand
    {
        public double LinearVelocity { get; set; }
        public Angle AngularVelocity { get; set; }
        public double Duration { get; set; }
        public string Command { get; set; }
        public bool WaitForExit { get; set; }


    }
}
