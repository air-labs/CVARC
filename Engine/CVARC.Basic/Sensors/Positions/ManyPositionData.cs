using System.Collections.Generic;
using System.Linq;

namespace CVARC.Basic.Sensors
{
    public class ManyPositionData : ISensorData
    {
        private readonly List<PositionData> _data;

        public ManyPositionData(List<PositionData> data)
        {
            _data = data;
        }

        public string GetStringRepresentation()
        {
            return "<Robots>" + _data.Select(z => z.GetStringRepresentation()).Aggregate((a, b) => a + b) + "</Robots>";
        }
    }
}