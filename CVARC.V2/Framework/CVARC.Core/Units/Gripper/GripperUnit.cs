using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;

namespace CVARC.V2
{

    public class GrippingAvailability
    {
        public readonly Frame3D RelativeLocation;
        public readonly double Distance;
        public readonly Angle Angle;
        public GrippingAvailability(Frame3D relativeLocation)
        {
            RelativeLocation=relativeLocation;
            Distance = Math.Sqrt(Math.Pow(RelativeLocation.X, 2) + Math.Pow(RelativeLocation.Y, 2));
            Angle = Angle.FromRad(Math.Atan2(RelativeLocation.Y, relativeLocation.X));
        }
    }

    public class GripperUnit
    {
        IActor actor;

		public const string GripCommand = "Grip";
		public const string ReleaseCommand = "Release";

        public GripperUnit(IActor actor)
        {
            this.actor = actor;
        }

        public string GrippedObjectId { get; private set; }

        public Frame3D GrippingPoint { get; set; }

        public Func<string> FindDetail { get; set; }

        public void Grip()
        {
            if (GrippedObjectId != null) return;
            var objectId = FindDetail(); 
            if (objectId == null) return;
            GrippedObjectId = objectId;
            actor.World.Engine.Attach(
                GrippedObjectId,
                actor.ObjectId,
                GrippingPoint
                );
        }

        public GrippingAvailability GetAvailability(string objectId)
        {
            if (!actor.World.Engine.ContainBody(objectId)) return null;
            var robotLocation = actor.World.Engine.GetAbsoluteLocation(actor.ObjectId);
            var gripperLocation = robotLocation.Apply(GrippingPoint);
            var objectLocation = actor.World.Engine.GetAbsoluteLocation(objectId);
            var relativeLocation = gripperLocation.Invert().Apply(objectLocation);
            return new GrippingAvailability(relativeLocation);
        }

        public void Release()
        {
            if (GrippedObjectId == null) return;
            var detailId = GrippedObjectId;
            GrippedObjectId = null;
            var location = actor.World.Engine.GetAbsoluteLocation(detailId);
            actor.World.Engine.Detach(detailId, location);
        }
    }
}
