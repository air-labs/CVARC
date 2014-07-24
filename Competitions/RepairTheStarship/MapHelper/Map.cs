using System.Collections.Generic;
using RepairTheStarship.Sensors;

namespace MapHelper
{
    public class Map
    {
        public Wall[] Walls { get; set; }
        public Direction[,] AvailableDirectionsByCoordinates { get; set; }

        public void Update(MapItem[] mapItems)
        {
            
        }
    }
}