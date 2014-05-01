using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Basic.Sensors;
using CVARC.Basic.Sensors.Positions;
using CVARC.Core;
using Gems.Sensors;
using StarshipRepair;
using System;
using System.Linq;

namespace Gems
{
    public class GemsRobot : Robot
    {
        private RobotIdSensor robotIdSensor;
        private MapSensor mapSensor;
        private LightHouseSensor lightHouseSensor;

        public GemsRobot(GemsWorld world)
            : base(world)
        {
        }

        public override void Init()
        {
            robotIdSensor = new RobotIdSensor(this, World, World.DrawerFactory);
            mapSensor = new MapSensor(this, World, World.DrawerFactory);
            lightHouseSensor = new LightHouseSensor(this, World, World.DrawerFactory);
        }

        public override ISensorsData GetSensorsData()
        {
            return new SensorsData
                {
                    RobotIdSensor = robotIdSensor.Measure(),
                    LightHouseSensor = lightHouseSensor.Measure(),
                    MapSensor = mapSensor.Measure()
                };
        }

        public override void ProcessCommand(Command cmd)
        {
            RequestedSpeed = GetRequestedSpeed(cmd);
            switch (cmd.Cmd)
            {
                case Cmd.Grip:
                    Grip();
                    break;
                case Cmd.Release:
                    Release();
                    break;
                case null:
                    break;
                default:
                    throw new Exception("Unknown command");
            }
        }

        private Frame3D GetRequestedSpeed(Command cmd)
        {
            return new Frame3D(cmd.Move*Math.Cos(Body.Location.Yaw.Radian),
                               cmd.Move*Math.Sin(Body.Location.Yaw.Radian), 0, Angle.Zero, cmd.Angle,
                               Angle.Zero);
        }

        private void Release()
        {
            var latestGripped = Body.FirstOrDefault(z => z.Name.StartsWith("D") && z.Name.Length == 2);
            if (latestGripped == null) return;

            var absoluteLocation = latestGripped.GetAbsoluteLocation();
            Body.Remove(latestGripped);

            var targetColor = latestGripped.Name[1].ToString();

            latestGripped.Location = absoluteLocation;
            latestGripped.Velocity = Body.Velocity;
            var toAtt = Body.TreeRoot.GetSubtreeChildrenFirst()
                            .Where(a =>
                                    (a.Name == "VW" + targetColor || a.Name == "HW" + targetColor) &&
                                    Distance(latestGripped, a) < 30)
                            .OfType<Box>()
                            .FirstOrDefault();

            if (toAtt != null)
            {
                Body.TreeRoot.Remove(toAtt);
                var wall = new Box
                {
                    Name = toAtt.Name.Substring(0, 2),
                    XSize = toAtt.XSize,
                    YSize = toAtt.YSize,
                    ZSize = toAtt.ZSize,
                    Location = toAtt.Location,
                    DefaultColor = GemsWorld.WallColor,
                    IsStatic = true,
                    IsMaterial = true
                };
                Body.TreeRoot.Add(wall);
                AddScore(10, "Repaired wall " + targetColor);
            }
            else
                Body.TreeRoot.Add(latestGripped);
            //  gripped.RemoveRange(0, gripped.Count);
        }

        private void Grip()
        {
            var gripped = Body.ToList();
            if (gripped.Any()) return;
            var found = Body.TreeRoot.GetSubtreeChildrenFirst().FirstOrDefault(a => CanBeAttached(Body, a) && (a.Parent == a.TreeRoot || !a.Parent.IsStatic));
            if (found != null)
            {
                Body latestGripped = null;
                var oldGrippedCount = gripped.Count;
                if (oldGrippedCount > 0)
                {
                    latestGripped = gripped.Last();
                    if (found.DefaultColor != latestGripped.DefaultColor)
                        return;
                }

                var tempfound = found;

                while (tempfound.Any())
                {
                    tempfound = tempfound.FirstOrDefault();
                    gripped.Add(tempfound);
                }

                if (oldGrippedCount > 0)
                {
                    Body.Remove(latestGripped);
                    if (latestGripped != null)
                    {
                        latestGripped.Location = new Frame3D(0, 0, 8, Angle.Zero, Angle.Zero, Angle.Zero);
                        tempfound.Add(latestGripped);
                    }
                }

                CaptureDevicet(Body, found);
                gripped.Add(found);
            }

        }

        private bool CanBeAttached(Body to, Body body)
        {
            return body != to &&
                !body.IsStatic &&
                !to.SubtreeContainsChild(body) &&
                !to.ParentsContain(body) &&
                body.Name.StartsWith("D") &&
                Distance(body, to) < 30;
        }

        private double Distance(Body from, Body to)
        {
            return Angem.Hypot(from.GetAbsoluteLocation() - to.GetAbsoluteLocation());
        }


        private void CaptureDevicet(Body box, Body newChild)
        {
            var childAbsolute = newChild.GetAbsoluteLocation();
            if (newChild.Parent != null)
                newChild.Parent.Remove(newChild);
            newChild.Location = box.GetAbsoluteLocation().Invert().Apply(childAbsolute);
            newChild.Location = newChild.Location.NewYaw(Angle.Zero);
            newChild.Location = newChild.Location.NewX(14);
            newChild.Location = newChild.Location.NewY(0);
            box.Add(newChild);
        }
    }
}
