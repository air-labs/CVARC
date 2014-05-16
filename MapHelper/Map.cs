using System.Collections.Generic;
using Gems.Sensors;

namespace MapHelper
{
    public class Map
    {
        public List<Wall> Walls { get; set; }
        public Direction[,] BitArray { get; set; }

        public void Update(MapItem[] mapItems)
        {
            
        }
    }
}