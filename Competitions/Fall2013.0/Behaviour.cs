using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Basic.Sensors;
using CVARC.Core;
using kinect.Integration;

namespace Gems
{
    public class Behaviour : RobotBehaviour
    {
        public override void InitSensors()
        {
 	        // Add<RobotCamera>();
 	        // Add<Kinect>();
 	         Add<LightHouseSensor>();
 	         Add<MapSensor>();
        }
        public Behaviour()
        {
            CommandRecieved += OnCommandRecieved;
        }

        private void OnCommandRecieved(Robot robot, Command command)
        {
            if(command.Cmd == "Grip")
            {
                Grip(robot);
            }
            if(command.Cmd == "Release")
            {
                Release(robot);
            }
        }

        private static void Release(Robot robot)
        {
            var latestGripped = robot.Body.Where(z => z.Name.StartsWith("D") && z.Name.Length==2).FirstOrDefault();
            if (latestGripped == null) return;

            var absoluteLocation = latestGripped.GetAbsoluteLocation();
            robot.Body.Remove(latestGripped);

            var targetColor = latestGripped.Name[1].ToString();

            latestGripped.Location = absoluteLocation;
            latestGripped.Velocity = robot.Body.Velocity;
            var toAtt = robot.Body.TreeRoot.GetSubtreeChildrenFirst()
                            .Where(a =>
                                    (a.Name == "VW" + targetColor || a.Name == "HW" + targetColor) &&
                                    Distance(latestGripped, a) < 30)
                            .OfType<Box>()
                            .FirstOrDefault();

            if(toAtt != null)
            {
                robot.Body.TreeRoot.Remove(toAtt);
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
                robot.Body.TreeRoot.Add(wall);
                robot.AddScore(10,"Repaired wall "+targetColor);
            }
            else
                robot.Body.TreeRoot.Add(latestGripped);
          //  gripped.RemoveRange(0, gripped.Count);
        }

        private static void Grip(Robot robot)
        {
            var gripped = robot.Body.ToList();
            if(gripped.Any()) return;
            var found = robot.Body.TreeRoot.GetSubtreeChildrenFirst().FirstOrDefault(a => CanBeAttached(robot.Body, a) && (a.Parent == a.TreeRoot || !a.Parent.IsStatic));
            if (found != null)
            {
                Body latestGripped = null;
                var oldGrippedCount = gripped.Count;
                if (oldGrippedCount > 0)
                {
                    latestGripped = gripped.Last();
                    if (found.DefaultColor != latestGripped.DefaultColor)
                    {
                        return;
                    }
                }

                var tempfound = found;

                while (tempfound.Any())
                {
                    tempfound = tempfound.FirstOrDefault();
                    gripped.Add(tempfound);
                }

                if (oldGrippedCount > 0)
                {
                    robot.Body.Remove(latestGripped);
                    if(latestGripped != null)
                    {
                        latestGripped.Location = new Frame3D(0, 0, 8, Angle.Zero, Angle.Zero, Angle.Zero);
                        tempfound.Add(latestGripped);
                    }
                }

                CaptureDevicet(robot.Body, found);
                gripped.Add(found);
            }

        }

        public static void CaptureDevicet(Body box, Body newChild)
        {
            var childAbsolute = newChild.GetAbsoluteLocation();
            if (newChild.Parent != null)
            {
                newChild.Parent.Remove(newChild);
            }
            newChild.Location = box.GetAbsoluteLocation().Invert().Apply(childAbsolute);
            newChild.Location = newChild.Location.NewYaw(Angle.Zero);
            newChild.Location = newChild.Location.NewX(14);
            newChild.Location = newChild.Location.NewY(0);

            box.Add(newChild);
        }

        private static bool CanBeAttached(Body to, Body body)
        {
            return body != to &&
                !body.IsStatic &&
                !to.SubtreeContainsChild(body) &&
                !to.ParentsContain(body) &&
                body.Name.StartsWith("D") &&
                Distance(body, to) < 30;
        }
        private static double Distance(Body from, Body to)
        {
            return Angem.Hypot(from.GetAbsoluteLocation() - to.GetAbsoluteLocation());
        }

    }
}
