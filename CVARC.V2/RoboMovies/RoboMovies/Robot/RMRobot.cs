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
        SideColor robotColor;

		public override void AdditionalInitialization()
		{
			base.AdditionalInitialization();
            
            base.TowerBuilder.FindCollectable = () =>
                {
                    Func<string, bool> isAttachedToStand = s =>
                        (s = World.Engine.FindParent(s)) != null &&
                        World.IdGenerator.KeyOfType<RMObject>(s) &&
                        World.IdGenerator.GetKey<RMObject>(s).Type == ObjectType.Stand;

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
			
            base.TowerBuilder.GrippingPoint = new Frame3D(15, 0, 5);
            base.TowerBuilder.OnRelease = CheckTowerPosition;

            robotColor = ControllerId == TwoPlayersId.Left ? SideColor.Yellow : SideColor.Green;
		}
        
        private double Distance(string from, string to)
        {
            return Geometry.Hypot(World.Engine.GetAbsoluteLocation(from) - World.Engine.GetAbsoluteLocation(to));
        }

        void CheckTowerPosition(HashSet<string> tower, Frame3D location)
        {
            if (World.IsInsideStartingArea(location, robotColor) || World.IsInsideBuildingArea(location))
                foreach (var item in tower)
                    World.Scores.Add(ControllerId, 2, 
                        String.Format("{0} stand(s) has been deployed in correct place.", tower.Count));
        }
    }
}
