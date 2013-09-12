using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.Basic
{
    public class EmptyBot : Bot
    {
        public override Controllers.Command MakeTurn()
        {
            return new Controllers.Command { Time = 1 };
        }
    }
}
