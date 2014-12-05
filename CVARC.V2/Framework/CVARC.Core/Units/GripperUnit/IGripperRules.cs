﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2.Units
{
	public interface IGripperRules
	{
        double GrippingTime { get; }
        double ReleasingTime { get; }
	}

    public interface IGripperRules<TCommand> : IGripperRules
    {
    }
}
