using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab;
using AIRLab.Mathematics;
using CVARC.Basic.Sensors;
using CVARC.V2;

namespace RoboMovies
{
    public class InternalMapSensor : Sensor<MapItem[],IActor>
    {
        string GetType(string id)
        {
            if (Actor.World.IdGenerator.KeyOfType<WallData>(id))
            {
                var wallData = Actor.World.IdGenerator.GetKey<WallData>(id);
                return wallData.Orientation.ToString() + wallData.Type.ToString();
            }
            if (Actor.World.IdGenerator.KeyOfType<DetailColor>(id))
            {
                return Actor.World.IdGenerator.GetKey<DetailColor>(id).ToString() + "Detail";
            }
            return null;
        }


        public override MapItem[] Measure()
        {
            return Actor.World.IdGenerator.GetAllId()
                .Select(z => new Tuple<string, string>(z, GetType(z)))
                .Where(z => z.Item2 != null)
                .Where(z=>Actor.World.Engine.ContainBody(z.Item1))
                .Select(z => new Tuple<string, Frame3D>(z.Item2, Actor.World.Engine.GetAbsoluteLocation(z.Item1)))
                .Select(z => new MapItem
                {
                    Tag = z.Item1,
                    X = z.Item2.X,
                    Y = z.Item2.Y
                })
                .ToArray();
        }
    }
}
