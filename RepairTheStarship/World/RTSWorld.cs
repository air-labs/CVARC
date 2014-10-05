using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace RepairTheStarship
{
    public class RTSWorld : World<SceneSettings,IRTSWorldManager>
    {
        protected override IEnumerable<IActor> CreateActors()
        {
            yield break;
        }
    }
}
