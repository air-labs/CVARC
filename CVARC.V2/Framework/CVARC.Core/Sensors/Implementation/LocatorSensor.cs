using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab;
using AIRLab.Mathematics;

namespace CVARC.V2
{
    public class LocatorSensor : Sensor<LocatorItem[],IActor>
    {
        public override LocatorItem[] Measure()
        {
            return Actor.World.Actors
                .OfType<IActor>()
                .Where(z=>Actor.World.Engine.ContainBody(z.ObjectId))
                .Select(z => new Tuple<string, Frame3D>(
                    z.ControllerId,
                    Actor.World.Engine.GetAbsoluteLocation(z.ObjectId)))
                .Select(z => new LocatorItem
                {
                    Id = z.Item1,
                    X = z.Item2.X,
                    Y = z.Item2.Y,
                    Angle = z.Item2.Yaw.Grad
                })
                .ToArray();
        }
    }
}
