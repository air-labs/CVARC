using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace RoboMovies
{
    public interface IRMRobot : ITowerBuilderRobot, IGrippableRobot, IRMCombinedRobot, IActor
    {
    }
}
