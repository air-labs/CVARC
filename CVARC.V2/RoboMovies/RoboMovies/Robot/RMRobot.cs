﻿using System;
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
                        GetObjectFromId(s).Type == ObjectType.Stand;

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
            base.TowerBuilder.OnGrip = (id, location) =>
                {
                    var obj = GetObjectFromId(id);
                    if (obj.Color != robotColor && obj.Color != SideColor.Any)
                        World.Scores.Add(ControllerId, -10, "Took the stand of invalid color.");
                };

            robotColor = ControllerId == TwoPlayersId.Left ? SideColor.Yellow : SideColor.Green;
		}
        
        private double Distance(string from, string to)
        {
            return Geometry.Hypot(World.Engine.GetAbsoluteLocation(from) - World.Engine.GetAbsoluteLocation(to));
        }

        void CheckTowerPosition(HashSet<string> tower, Frame3D location)
        {
            var bonus = TowerBuilder.ContainsBall ? 3 : 0;

            var validTower = tower
                .Select(x => GetObjectFromId(x))
                .Where(x => x.Color == robotColor && x.Type == ObjectType.Stand);

            if (World.IsInsideStartingArea(location, robotColor) || World.IsInsideBuildingArea(location))
                foreach (var item in validTower)
                    World.Scores.Add(ControllerId, 2 + bonus, 
                        String.Format("{0} stand(s) has been deployed in correct place.", tower.Count));
        }

        RMObject GetObjectFromId(string id)
        {
            if (!World.IdGenerator.KeyOfType<RMObject>(id))
                throw new ArgumentException("This id is not bind to any RMObject.");
            return World.IdGenerator.GetKey<RMObject>(id);
        }
    }
}
