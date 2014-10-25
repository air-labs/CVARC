using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2.SimpleMovement;

namespace RepairTheStarship
{
    public class RTSCommandPreprocessor : SimpleMovementPreprocessor
    {
        public override int GetDurationForCustomCommand(SimpleMovementCommand command)
        {
            return 1;
        }
    }
}
