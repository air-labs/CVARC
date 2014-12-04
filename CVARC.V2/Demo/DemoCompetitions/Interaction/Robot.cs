using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2.SimpleMovement;
using CVARC.V2;
using RepairTheStarship;

namespace Demo
{
    public class InteractionRobot<TSensorsData> : SimpleMovementRobot<IActorManager, MovementWorld, InteractionSensorData>, IRTSRobot
        where TSensorsData : new()
    {

        
        
        public string GrippedObjectId { get; private set; }


        private double Distance(string from, string to)
        {
            return Geometry.Hypot(World.Engine.GetAbsoluteLocation(from) - World.Engine.GetAbsoluteLocation(to));
        }

        private bool IsDetailAheadRobot(string _robot, string _detail)
        {
            Frame3D robot = World.Engine.GetAbsoluteLocation(_robot);
            Frame3D detail = World.Engine.GetAbsoluteLocation(_detail);
            var angle = robot.Yaw.Grad;
            while (angle < 0)
                angle += 360;
            while (angle > 360)
                angle -= 360;
            const int angleLatitude = 40;
            const int detailDistance = 10;
            var detailAbove = detail.Y > robot.Y && Math.Abs(detail.Y - robot.Y) > detailDistance && angle >= 90 - angleLatitude && angle <= 90 + angleLatitude;
            var detailBelow = detail.Y < robot.Y && Math.Abs(detail.Y - robot.Y) > detailDistance && angle >= 270 - angleLatitude && angle <= 270 + angleLatitude;
            var detailLeft = detail.X < robot.X && Math.Abs(detail.X - robot.X) > detailDistance && angle >= 180 - angleLatitude && angle <= 180 + angleLatitude;
            var detailRight = detail.X > robot.X && Math.Abs(detail.X - robot.X) > detailDistance && ((angle >= 360 - angleLatitude && angle <= 360) || (angle >= 0 && angle <= angleLatitude));
            return detailAbove || detailBelow || detailLeft || detailRight;
        }
        void Grip()
        {
            if (GrippedObjectId != null) return;
            var found = World.IdGenerator.GetAllPairsOfType<string>()
                .Where(z => World.Engine.ContainBody(z.Item2))
                .Where(z => !World.Engine.IsAttached(z.Item2))
                .Where(z => Distance(ObjectId, z.Item2) < 30)
                .Where(z => IsDetailAheadRobot(ObjectId, z.Item2))
                .FirstOrDefault();

            if (found == null) return;
            GrippedObjectId = found.Item2;
            World.Engine.Attach(
                GrippedObjectId,
                ObjectId,
                new Frame3D(14, 0, 0, Angle.Zero, Angle.Zero, Angle.Zero)
                );
        }
        void Release()
        {
            if (GrippedObjectId == null) return;
            var detailId = GrippedObjectId;
            GrippedObjectId = null;
            var location = World.Engine.GetAbsoluteLocation(detailId);
            World.Engine.Detach(detailId, location);
        }

        public override void ProcessCustomCommand(string commandName)
        {
            if (commandName == RTSAction.Grip.ToString())
                Grip();
            else if (commandName == RTSAction.Release.ToString())
                Release();
        }

    }
}
