using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public delegate void RenewableTriggerDelegate(RenewableTriggerData data, out double nextCallTime);
}
