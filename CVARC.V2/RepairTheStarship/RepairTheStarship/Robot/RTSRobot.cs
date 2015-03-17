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
		}
    }
}
