using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.V2;
using CVARC.V2;

namespace RepairTheStarship
{
    public class RTSRobot<TSensorsData> : MoveAndGripRobot<IRTSActorManager,RTSWorld,TSensorsData>
        where TSensorsData : new()
    {
		public override void AdditionalInitialization()
		{
			base.AdditionalInitialization();
            base.Gripper.FindDetail = () =>
                {
                    var allGrippable =
                World.IdGenerator.GetAllPairsOfType<DetailColor>()
                .Where(z => World.Engine.ContainBody(z.Item2))
                .Where(z => !World.Engine.IsAttached(z.Item2))
                .Select(z => new { Item = z, Availability = Gripper.GetAvailability(z.Item2) })
                .ToList();

                    return allGrippable
                .Where(z => z.Availability.Distance < 30)
                .OrderBy(z => z.Availability.Distance)
                .Select(z => z.Item.Item2)
                .FirstOrDefault();
                };
			base.Gripper.GrippingPoint = new Frame3D(15, 0, 10);

            base.Gripper.OnRelease = Release;

		}

        private double Distance(string from, string to)
        {
            return Geometry.Hypot(World.Engine.GetAbsoluteLocation(from) - World.Engine.GetAbsoluteLocation(to));
        }

        void Release(string detailId, Frame3D location)
        {
            var detailColor = World.IdGenerator.GetKey<DetailColor>(detailId);

            var wall = World.IdGenerator.GetAllPairsOfType<WallData>()
                .Where(z => World.Engine.ContainBody(z.Item2))
                .Where(z => z.Item1.Match(detailColor))
                .Where(z => Distance(detailId, z.Item2) < 30)
                .FirstOrDefault();

            World.Engine.Detach(detailId, location);
            if (wall != null)
                World.InstallDetail(detailColor, detailId, wall.Item2, ControllerId);
        }
    }
}
