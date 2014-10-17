using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jango
{
    public class JangoWorld : World<object, JangoWorldManager>
    {
        public override IEnumerable<string> ControllersId
        {
            get { yield return "Robot"; }
        }

        public override IActor CreateActor(string controllerId)
        {
            return new JangoRobot(controllerId);
        }

        public override object CreateSceneState(int seed)
        {
            return new object();
        }
    }
}
