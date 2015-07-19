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

    public abstract class BaseGripperUnit<TRules> : IUnit
    {
        protected IActor actor;
        protected TRules rules;
        
        public BaseGripperUnit(IActor actor)
        {
            this.actor = actor;
            rules = Compatibility.Check<TRules>(this, actor.Rules);
        }
        
        public Frame3D GrippingPoint { get; set; }
        public virtual Frame3D RobotLocation { get { return actor.World.Engine.GetAbsoluteLocation(actor.ObjectId); } }
        
        public virtual GrippingAvailability GetAvailability(string objectId)
        {
            if (!actor.World.Engine.ContainBody(objectId)) return null;
            var gripperLocation = RobotLocation.Apply(GrippingPoint);
            var objectLocation = actor.World.Engine.GetAbsoluteLocation(objectId);
            var relativeLocation = gripperLocation.Invert().Apply(objectLocation);
            return new GrippingAvailability(relativeLocation);
        }
        
        public abstract UnitResponse ProcessCommand(Object command);
    }
}
