﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace RepairTheStarship
{
    public interface IRTSRobot : IActor
    {
        string GrippedObjectId { get; }
    }
}
