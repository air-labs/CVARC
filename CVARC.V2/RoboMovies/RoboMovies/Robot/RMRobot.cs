using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.V2;

namespace RoboMovies
{
    public class RMRobot<TSensorsData> : 
                Robot<IRMActorManager, RMWorld, TSensorsData, RMCommand, RMRules>,
                ITowerBuilderRobot, IGrippableRobot, IRMCombinedRobot
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
                yield return SimpleMovementUnit;
                yield return TowerBuilder;
                yield return Gripper;
                yield return Combiner;
            }
        }
    
		public override void AdditionalInitialization()
		{
            base.AdditionalInitialization();
            SimpleMovementUnit = new SimpleMovementUnit(this);
            TowerBuilder = new TowerBuilderUnit(this);
            Gripper = new GripperUnit(this);
            Combiner = new RMCombinedUnit(this);

            var robotColor = ControllerId == TwoPlayersId.Left ? SideColor.Yellow : SideColor.Green;

            #region Combiner
            Combiner.FindClapperboards = () =>
                {
                    return World.IdGenerator.GetAllPairsOfType<RMObject>()
                        .Where(z => z.Item1.Type == ObjectType.Clapperboard)
                        .Where(z => World.Engine.ContainBody(z.Item2))
                        .Select(z => z.Item2);
                };
            #endregion

            #region TowerBuilder
            TowerBuilder.FindCollectable = () =>
                {
                    Func<string, bool> isAttachedToStand = s =>
                        (s = World.Engine.FindParent(s)) != null &&
                        World.IdGenerator.KeyOfType<RMObject>(s) &&
                        World.GetObjectById(s).Type == ObjectType.Stand;

                    var allCollectable = World.IdGenerator.GetAllPairsOfType<RMObject>()
                		.Where(z => z.Item1.Type == ObjectType.Stand || z.Item1.Type == ObjectType.Light)
                		.Where(z => World.Engine.ContainBody(z.Item2))
                		.Where(z => isAttachedToStand(z.Item2) || !World.Engine.IsAttached(z.Item2)) 
                        .OrderByDescending(z => World.Engine.GetAbsoluteLocation(z.Item2).Z)
                		.Select(z => new { Id = z.Item2, Availability = TowerBuilder.GetAvailability(z.Item2) })
                		.ToList();

                    return allCollectable
                        .Where(z => z.Availability.Distance < 10)
                        .Select(z => z.Id);
                };
            
            TowerBuilder.GrippingPoint = new Frame3D(15, 0, 5);
            
            TowerBuilder.OnGrip = (id, location) =>
                {
                    var obj = World.GetObjectById(id);
                    if (obj.Color != robotColor && obj.Color != SideColor.Any)
                        World.Scores.Add(ControllerId, -10, "Took the stand of invalid color.");

                    World.Spotlights = World.Spotlights
                        .Where(kv => !kv.Value.Contains(id))
                        .ToDictionary(kv => kv.Key, kv => kv.Value);
                };

            TowerBuilder.OnRelease = (ids, location) => 
            {
                var light = ids
                    .Where(id => World.IdGenerator.GetKey<RMObject>(id).Type == ObjectType.Light)
                    .FirstOrDefault();
                ids.Remove(light);
                if (light != null) World.Spotlights[light] = ids;
            };
            #endregion

            #region Gripper
            Gripper.FindDetail = () =>
                {
                    var allCollectable = World.IdGenerator.GetAllPairsOfType<RMObject>()
                		.Where(z => z.Item1.Type == ObjectType.PopCorn)
                		.Where(z => World.Engine.ContainBody(z.Item2))
                		.Where(z => !World.Engine.IsAttached(z.Item2)) 
                        .OrderByDescending(z => World.Engine.GetAbsoluteLocation(z.Item2).Z)
                		.Select(z => new { Id = z.Item2, Availability = Gripper.GetAvailability(z.Item2) })
                		.ToList();

                    return allCollectable
                        .Where(z => z.Availability.Distance < 10)
                        .OrderBy(z => z.Availability.Distance)
                        .Select(z => z.Id)
                        .FirstOrDefault();
                };
            
            Gripper.GrippingPoint = new Frame3D(-15, 0, 5);
            Gripper.OnRelease = (id, location) => World.Engine.Detach(id, location.NewZ(3));
            #endregion
        }
        
        double Distance(string from, string to)
        {
            return Geometry.Hypot(World.Engine.GetAbsoluteLocation(from) - World.Engine.GetAbsoluteLocation(to));
        }
    }
}
