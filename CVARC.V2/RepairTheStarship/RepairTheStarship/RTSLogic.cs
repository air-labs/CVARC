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
                () => new RTSKeyboardControllerPool())
        {
            Bots[RepairTheStarshipBots.Azura.ToString()] = () => new Azura();
            Bots[RepairTheStarshipBots.Vaermina.ToString()] = () => new Vaermina();
            Bots[RepairTheStarshipBots.MolagBal.ToString()] = () => new MolagBal();
            Bots[RepairTheStarshipBots.Sanguine.ToString()] = () => new Sanguine();

        }

        public override Settings GetDefaultSettings()
        {
            return new Settings { OperationalTimeLimit = 1, TimeLimit = 90 };
        }
    }
}
