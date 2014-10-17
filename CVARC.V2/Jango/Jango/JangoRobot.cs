using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jango
{
    public class JangoRobot : Robot<JangoActorManager, JangoWorld, object, JangoCommand>
    {
        public JangoRobot(string controllerId)
            : base(controllerId)
        { }

        public override void ExecuteCommand(JangoCommand command)
        {
            
        }
    }
}
