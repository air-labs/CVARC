using AIRLab.Mathematics;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jango
{
    public class JangoCommand : ICommand
    {
        public Angle[] RequestedAngleDeltas { get; set; }

        public double Duration
        {
            get;
            set;
        }
    }
}
