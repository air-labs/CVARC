using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace RepairTheStarship
{
    public class RTSLogicPart : LogicPart
    {
        public RTSLogicPart()
            : base(
                new RTSWorld(),
                (keyboard)=>new RTSKeyboardControllerPool(keyboard),
                seed => SceneSettings.GetRandomMap(seed))
        { }
    }
}
