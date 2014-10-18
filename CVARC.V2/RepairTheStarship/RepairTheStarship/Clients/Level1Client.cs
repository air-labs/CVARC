using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepairTheStarship
{
    public class Level1Client : RTSClient<Level1SensorData>
    {
        public Level1Client(bool runServer)
            : base(runServer)
        { }

        public Level1SensorData Configurate(bool isOnLeftSide)
        {
            return Configurate(isOnLeftSide, RepairTheStarshipBots.None);
        }

        public override string LevelName
        {
            get { return "Level1"; }
        }
    }
}
