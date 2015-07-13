using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;

namespace CVARC.V2
{
    public class TowerBuilderUnit : IUnit
    {
        IActor actor;
        ITowerBuilderRules rules;
        Queue<string> collectedIds;
        double detailHeight = 7;

        public TowerBuilderUnit(IActor actor)
        {
            this.actor = actor;
            this.rules = Compatibility.Check<ITowerBuilderRules>(this, actor.Rules);
            collectedIds = new Queue<string>();
        }

        public int Capacity { get { return rules.BuilderCapacity; } }
        
        public IEnumerable<string> CollectedIds { get { return collectedIds; } }
        public string LastCollectedId { get { return collectedIds.Peek(); } }
        public int CollectedCount { get { return collectedIds.Count; } }
        
        public Frame3D GrippingPoint { get; set; }
        public Frame3D RobotLocation { get { return actor.World.Engine.GetAbsoluteLocation(actor.ObjectId); } }

        public Func<IEnumerable<string>> FindCollectable { get; set; }
        public Action<IEnumerable<string>, Frame3D> OnTowerBuild { get; set; }
        public Action<string, Frame3D> OnCollecting { get; set; }

        public GrippingAvailability GetAvailability(string objectId)
        {
            if (!actor.World.Engine.ContainBody(objectId)) return null;
            var gripperLocation = RobotLocation.Apply(GrippingPoint);
            var objectLocation = actor.World.Engine.GetAbsoluteLocation(objectId).NewZ(RobotLocation.Z);
            var relativeLocation = gripperLocation.Invert().Apply(objectLocation);
            return new GrippingAvailability(relativeLocation);
        }

        void GrabObject(string objectId)
        {
            var connectionPoint = GrippingPoint + new Frame3D(0, 0, CollectedCount * detailHeight);
            actor.World.Engine.Attach(objectId, actor.ObjectId, connectionPoint);
            collectedIds.Enqueue(objectId);
        }

        void Collect()
        {
            foreach (var itemId in FindCollectable())
            {
                var location = actor.World.Engine.GetAbsoluteLocation(itemId).NewZ(RobotLocation.Z);
                
                if (actor.World.Engine.IsAttached(itemId))
                    actor.World.Engine.Detach(itemId, location);

                if (CollectedCount < Capacity)
                {
                    GrabObject(itemId);
                    if (OnCollecting != null) OnCollecting(itemId, location);
                }
            }
        }

        void DetachCollected()
        {
            if (CollectedCount == 0) return;

            var dropLocation = actor.World.Engine
                .GetAbsoluteLocation(collectedIds.Peek())
                .NewZ(RobotLocation.Z);
            
            if (OnTowerBuild != null)
                OnTowerBuild(CollectedIds, dropLocation);
            
            actor.World.Engine.Detach(collectedIds.Peek(), dropLocation);

            BuildTower();
        }

        void BuildTower()
        {
            var first = collectedIds.Dequeue();
            var index = 1;

            while(collectedIds.Count > 0)
                Reattach(collectedIds.Dequeue(), first, new Frame3D(0, 0, detailHeight * index++));
        }

        void Reattach(string objectToReattach, string newHost, Frame3D connectionPoint)
        {
            actor.World.Engine.Detach(objectToReattach, actor.World.Engine.GetAbsoluteLocation(objectToReattach));
            actor.World.Engine.Attach(objectToReattach, newHost, connectionPoint);
        }

        public UnitResponse ProcessCommand(object _command)
        {
            var command = Compatibility.Check<ITowerBuilderCommand>(this, _command);
            Debugger.Log(DebuggerMessageType.Workflow, "Command comes to gripper, " + command.TowerBuilderCommand.ToString());
            switch (command.TowerBuilderCommand)
            {
                case TowerBuilderAction.No: return UnitResponse.Denied();
                case TowerBuilderAction.Collect: 
                    Collect(); 
                    return UnitResponse.Accepted(rules.CollectingTime);
                case TowerBuilderAction.BuildTower: 
                    DetachCollected(); 
                    return UnitResponse.Accepted(rules.BuildingTime);
                default: throw new Exception("Unrecognized gripper action.");
            }
        }
    }
}
