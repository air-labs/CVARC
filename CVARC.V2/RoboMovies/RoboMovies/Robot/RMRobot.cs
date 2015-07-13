using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.V2;

namespace RoboMovies
{
    public class RMRobot<TSensorsData> : MoveAndBuildRobot<IRMActorManager,RMWorld,TSensorsData>
        where TSensorsData : new()
    {
		public override void AdditionalInitialization()
		{
			base.AdditionalInitialization();
            
            base.TowerBuilder.FindCollectable = () =>
                {
                    Func<string, bool> isAttachedToStand = s =>
                        World.Engine.FindParent(s) != null &&
                        World.IdGenerator.KeyOfType<RMObject>(World.Engine.FindParent(s)) &&
                        World.IdGenerator.GetKey<RMObject>(s).Type == ObjectType.Stand;

                    var allStands = World.IdGenerator.GetAllPairsOfType<RMObject>()
                		.Where(z => z.Item1.Type == ObjectType.Stand)
                		.Where(z => World.Engine.ContainBody(z.Item2))
                		.Where(z => isAttachedToStand(z.Item2) || !World.Engine.IsAttached(z.Item2)) 
                		.Select(z => new { Id = z.Item2, Availability = TowerBuilder.GetAvailability(z.Item2) })
                		.ToList();

                    return allStands
                        .Where(z => z.Availability.Distance < 10)
                        .Select(z => z.Id);
                };
			
            base.TowerBuilder.GrippingPoint = new Frame3D(15, 0, 10);
            base.TowerBuilder.OnTowerBuild = CheckTowerPosition;
		}

        private double Distance(string from, string to)
        {
            return Geometry.Hypot(World.Engine.GetAbsoluteLocation(from) - World.Engine.GetAbsoluteLocation(to));
        }

        void CheckTowerPosition(IEnumerable<string> tower, Frame3D location)
        {

        }
    }
}
