using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using RoboMovies;

namespace CVARC.V2
{
    public class TowerBuilderUnit : BaseGripperUnit<ITowerBuilderRules>
    {
        public TowerBuilderUnit(IActor actor) : base(actor) { }
        
        Stack<string> stackStorage = new Stack<string>();
        double detailHeight = 7;

        public int Capacity { get { return rules.BuilderCapacity; } }
        
        public HashSet<string> CollectedIds = new HashSet<string>();
        public bool ContainsBall { get; private set; }
        public string LastCollectedId { get { return stackStorage.Peek(); } }
        public int CollectedCount { get { return stackStorage.Count; } }
        
        public Action<HashSet<string>, Frame3D> OnRelease { get; set; }
        public Action<string, Frame3D> OnGrip { get; set; }
        
        public Func<IEnumerable<string>> FindCollectable { get; set; }

        void ReorganizeStorage()
        {
            var index = 0;
            foreach(var id in stackStorage)
                Reattach(id, actor.ObjectId, GrippingPoint + new Frame3D(0, 0, detailHeight * index++));
        }

        void Collect()
        {
            foreach (var itemId in FindCollectable())
            {
                var location = actor.World.Engine.GetAbsoluteLocation(itemId).NewZ(RobotLocation.Z);
                
                if (actor.World.Engine.IsAttached(itemId))
                    actor.World.Engine.Detach(itemId, location);

                if (IsAvailableToCollect(itemId))
                {
                    stackStorage.Push(itemId);
                    CollectedIds.Add(itemId);
                    if (OnGrip != null) OnGrip(itemId, location);
                }
            }

            ReorganizeStorage();
        }

        bool IsAvailableToCollect(string itemId)
        {
            var hasEmptySpace = CollectedCount < Capacity;
            var allowBall = !ContainsBall && CollectedCount == 0;
            var isBall = actor.World.IdGenerator.GetKey<RMObject>(itemId).Type == ObjectType.Light;
            var canInsertBall = !isBall || isBall && allowBall;

            if (isBall && allowBall && hasEmptySpace) ContainsBall = true;

            return hasEmptySpace && canInsertBall;
        }

        void DetachCollected()
        {
            if (CollectedCount == 0) return;

            var dropLocation = actor.World.Engine.GetAbsoluteLocation(stackStorage.Peek()).NewZ(RobotLocation.Z);
            
            if (OnRelease != null) 
                OnRelease(CollectedIds, dropLocation);
            
            ContainsBall = false;
            CollectedIds.Clear();
            actor.World.Engine.Detach(BuildTower(), dropLocation);
        }

        string BuildTower()
        {
            var towerBase = stackStorage.Pop();
            var index = 1;

            while(stackStorage.Count > 0)
                Reattach(stackStorage.Pop(), towerBase, new Frame3D(0, 0, detailHeight * index++));

            return towerBase;
        }

        void Reattach(string objectToReattach, string newHost, Frame3D connectionPoint)
        {
            if (actor.World.Engine.IsAttached(objectToReattach))
                actor.World.Engine.Detach(objectToReattach, actor.World.Engine.GetAbsoluteLocation(objectToReattach));
            actor.World.Engine.Attach(objectToReattach, newHost, connectionPoint);
        }
        
        public override GrippingAvailability GetAvailability(string objectId)
        {
            if (!actor.World.Engine.ContainBody(objectId)) return null;
            var gripperLocation = RobotLocation.Apply(GrippingPoint);
            var objectLocation = actor.World.Engine.GetAbsoluteLocation(objectId).NewZ(RobotLocation.Z);
            var relativeLocation = gripperLocation.Invert().Apply(objectLocation);
            return new GrippingAvailability(relativeLocation);
        }

        public override UnitResponse ProcessCommand(object _command)
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
