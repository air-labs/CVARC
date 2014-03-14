using CVARC.Basic;
using StarshipRepair;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gems
{
    public class GemsRobot : Robot
    {
        public GemsRobot(GemsWorld world) : base(world) { }

        //TODO: запилить список сенсоров, и возвращать их агрегированную информацию
        public override object GetSensorsData()
        {
            throw new NotImplementedException();
        }

        //Перенести из behaviour
        public override void ProcessCommand(CVARC.Basic.Controllers.Command cmd)
        {
            throw new NotImplementedException();
        }
    }
}
