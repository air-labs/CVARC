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
            return string.Join("", _data.Select(a => "<data>"+a.GetStringRepresentation()+"</data>"));
        }
    }
}