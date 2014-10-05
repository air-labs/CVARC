using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic;

namespace CVARC.V2
{
    public interface ISimpleMovementWorld
    {
        double LinearVelocityLimit { get; }
        Angle AngularVelocityLimit { get; }
    }
}
