using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jango
{
    public class Level1LogicPart : LogicPart
    {
        public override Settings GetDefaultSettings()
        {
            return new Settings { TimeLimit = double.PositiveInfinity };
        }

        public Level1LogicPart()
            : base(new JangoWorld(), () => new JangoKeyboardControllerPool())
        { }
    }
}
