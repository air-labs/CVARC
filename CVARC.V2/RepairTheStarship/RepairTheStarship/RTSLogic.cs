using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using CVARC.V2.SimpleMovement;
using RepairTheStarship.Bots;

namespace RepairTheStarship
{
    public class RTSLogicPart : LogicPart
    {
        public RTSLogicPart()
            : base(
                new RTSWorld(),
                ()=>new RTSKeyboardControllerPool(),
                90)
        {
            Bots["Azura"] = () => new Azura();
            Bots["Vaermina"] = () => new Vaermina();
            Bots["MolagBal"] = () => new MolagBal();
            Bots["Sanguine"] = () => new Sanguine();
        
        }
    }
}
