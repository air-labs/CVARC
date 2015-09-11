using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.V2;

namespace RoboMovies
{
    public class RMRobot<TSensorsData> : Robot<IRMActorManager, RMWorld, TSensorsData, RMCommand, RMRules>, IRMRobot
        where TSensorsData : new()
    {
        public SimpleMovementUnit SimpleMovementUnit { get; private set; }
        public TowerBuilderUnit TowerBuilder {get; private set;}
        public GripperUnit Gripper { get; private set; }
        public RMCombinedUnit Combiner { get; set; }

        public override IEnumerable<IUnit> Units
        {
            get
            {
                yield return TowerBuilder;
                yield return Gripper;
                yield return Combiner;
                yield return SimpleMovementUnit;
            }
        }
    
		public override void AdditionalInitialization()
		{
            base.AdditionalInitialization();

            Debugger.Log(RMDebugMessage.Logic, "RMRobot initialization started...");

            SimpleMovementUnit = new SimpleMovementUnit(this);
            TowerBuilder = new TowerBuilderUnit(this);
            Gripper = new GripperUnit(this);
            Combiner = new RMCombinedUnit(this, World);

            var robotColor = ControllerId == TwoPlayersId.Left ? SideColor.Yellow : SideColor.Green;

            #region TowerBuilder
            Debugger.Log(RMDebugMessage.Logic, "    Initializing tower builder");
            TowerBuilder.FindCollectable = () =>
                {
                    Func<string, bool> isAttachedToStand = s =>
                        (s = World.Engine.FindParent(s)) != null &&
                        World.IdGenerator.KeyOfType<RMObject>(s) &&
                        World.GetObjectById(s).Type == ObjectType.Stand;

                    Debugger.Log(RMDebugMessage.Logic, "Searching for stands...");
                    var allStands = World.IdGenerator.GetAllPairsOfType<RMObject>()
                        .Where(z => z.Item1.Type == ObjectType.Stand || z.Item1.Type == ObjectType.Light)
                        .Where(z => World.Engine.ContainBody(z.Item2))
                        .ToArray();

                    foreach (var x in allStands)
                        Debugger.Log(RMDebugMessage.Logic, "    Stand found: " + x.Item2);
                    
                    Debugger.Log(RMDebugMessage.Logic, "Searching for collectable stands...");
                    var allCollectable = allStands
                		.Where(z => isAttachedToStand(z.Item2) || !World.Engine.IsAttached(z.Item2)) 
                        .OrderByDescending(z => World.Engine.GetAbsoluteLocation(z.Item2).Z)
                		.Select(z => new { Id = z.Item2, Availability = TowerBuilder.GetAvailability(z.Item2) })
                		.ToArray();

                    foreach (var x in allCollectable)
                        Debugger.Log(RMDebugMessage.Logic, "    Collectable stand found: " + x.Id);

                    return allCollectable
                        .Where(z => z.Availability.Distance < 10)
                        .Select(z => z.Id);
                };
            
            TowerBuilder.GrippingPoint = new Frame3D(16, 0, 5);
            
            TowerBuilder.OnGrip = (id, location) =>
                {
                    Debugger.Log(RMDebugMessage.Logic, String.Format("Gripping object {0} at {1}", id, location));

                    var obj = World.GetObjectById(id);
                    if (obj.Color != robotColor && obj.Color != SideColor.Any)
                        World.Scores.Add(ControllerId, -10, "Took the stand of invalid color.");

                    World.Spotlights = World.Spotlights
                        .Where(kv => !kv.Value.Contains(id))
                        .ToDictionary(kv => kv.Key, kv => kv.Value);
                };

            TowerBuilder.OnRelease = (ids, location) => 
            {
                foreach(var id in ids)
                    Debugger.Log(RMDebugMessage.Logic, String.Format("Releasing object {0} at {1}", id, location));

                var light = ids
                    .Where(id => World.IdGenerator.GetKey<RMObject>(id).Type == ObjectType.Light)
                    .FirstOrDefault();
                ids.Remove(light);
                if (light != null) World.Spotlights[light] = ids;
            };
            #endregion

            #region Gripper
            Debugger.Log(RMDebugMessage.Logic, "    Initializing gripper");
            Gripper.FindDetail = () =>
                {
                    Debugger.Log(RMDebugMessage.Logic, "Searching for collectable popcorn...");
                    
                    var allCollectable = World.IdGenerator.GetAllPairsOfType<RMObject>()
                		.Where(z => z.Item1.Type == ObjectType.PopCorn)
                		.Where(z => World.Engine.ContainBody(z.Item2))
                		.Where(z => !World.Engine.IsAttached(z.Item2)) 
                        .OrderByDescending(z => World.Engine.GetAbsoluteLocation(z.Item2).Z)
                		.Select(z => new { Id = z.Item2, Availability = Gripper.GetAvailability(z.Item2) })
                		.ToArray();

                    foreach (var x in allCollectable)
                        Debugger.Log(RMDebugMessage.Logic, "    Popcorn found: " + x.Id);

                    return allCollectable
                        .Where(z => z.Availability.Distance < 10)
                        .OrderBy(z => z.Availability.Distance)
                        .Select(z => z.Id)
                        .FirstOrDefault();
                };
            
            Gripper.GrippingPoint = new Frame3D(-17, 0, 5);
            Gripper.OnRelease = (id, location) => World.Engine.Detach(id, location.NewZ(3));
            #endregion

            Debugger.Log(RMDebugMessage.Logic, "RMRobot initialization finished!");
        }
    }
}
