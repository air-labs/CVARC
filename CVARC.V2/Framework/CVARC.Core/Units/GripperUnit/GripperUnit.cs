using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using System.Windows.Forms;

namespace CVARC.V2
{
    public class GripperUnit : BaseGripperUnit<IGripperRules>
    {
        public GripperUnit(IActor actor) : base(actor) { }

        public string GrippedObjectId { get; private set; }
        public Func<string> FindDetail { get; set; }
        public Action<string, Frame3D> OnRelease { get; set; }
        public Action<string, Frame3D> OnGrip { get; set; }
        
        void Grip()
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

        void Release()
        {
            if (GrippedObjectId == null) return;
            var detailId = GrippedObjectId;
            GrippedObjectId = null;
            var location = actor.World.Engine.GetAbsoluteLocation(detailId);
            if (OnRelease == null)
                actor.World.Engine.Detach(detailId, location);
            else
                OnRelease(detailId, location);
        }

        public override UnitResponse ProcessCommand(object _cmd)
        {
            var cmd = Compatibility.Check<IGripperCommand>(this, _cmd);
            Debugger.Log(DebuggerMessageType.Workflow,"Command comes to gripper, "+cmd.GripperCommand.ToString());
            switch (cmd.GripperCommand)
            {
                case GripperAction.No: return UnitResponse.Denied();
                case GripperAction.Grip:
                    Grip();
                    return UnitResponse.Accepted(rules.GrippingTime);
                case GripperAction.Release:
                    Release();
                    return UnitResponse.Accepted(rules.ReleasingTime);
            }
            throw new Exception("Cannot reach this part of code");
        }
    }
}
