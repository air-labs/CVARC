using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;

namespace DemoCompetitions
{
    class DemoWorld : World<object,IDemoWorldManagerPrototype>
    {
        protected override IEnumerable<IActor> CreateActors()
        {
            yield return new DemoRobot(0);
        }
    }
}
