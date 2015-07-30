using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace RoboMovies
{
    public class RTSLocationSensor : Sensor<RTSLocatorItem,IActor>
    {
        bool measureSelf;

        public RTSLocationSensor(bool self)
        {
            measureSelf = self;
        }

        public override RTSLocatorItem Measure()
        {
            var id = Actor.World.Actors
                .Where(z => (z.ControllerId == Actor.ControllerId) == measureSelf)
                .Select(z => z.ObjectId)
                .FirstOrDefault();
            if (id == null) throw new Exception("Robot is not found");
            if (!Actor.World.Engine.ContainBody(id)) throw new Exception("Robot is not in the world");
            var location = Actor.World.Engine.GetAbsoluteLocation(id);
            return new RTSLocatorItem
            {
                X = location.X,
                Y = location.Y,
                Angle = location.Yaw.Grad
            };
        }
    }

    public class SelfLocationSensor : RTSLocationSensor
    {
        public SelfLocationSensor() : base(true) { }
    }

    public class OpponentLocationSensor : RTSLocationSensor
    {
        public OpponentLocationSensor() : base(false) { }
    }



}
