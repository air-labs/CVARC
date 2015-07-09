using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRLab.Mathematics;
using CVARC.Core;
using CVARC.V2;

namespace RepairTheStarship.KroR
{
    public class RTSActorManager : ActorManager<IActor>, IRTSActorManager
    {

        public void EffectOnCapture(string detailId)
        {
            
        }

        public void EffectOnUnsuccessfullRelease(string detailId)
        {
        }

        public override void CreateActorBody()
        {
            var root = (Actor.World.Engine as KroREngine).Root;

            double X = -50;
            string fileName = "red.png";
            Body robot = null;

            if (Actor.ControllerId == TwoPlayersId.Left)
            {
                robot = new Cylinder
                 {
                     Height = 20,
                     RTop = 10,
                     RBottom = 10,
                     Location = new Frame3D(-150 + 25 - 10, 100 - 25 + 10, 3),
                     DefaultColor = Color.DarkViolet,
                     IsMaterial = true,
                     Density = Density.Iron,
                     FrictionCoefficient = 0,
                     Top = new PlaneImageBrush { Image = new Bitmap(GetResourceStream("red.png")) },
                     NewId = Actor.ObjectId
                 };
            }
            else
            {
                robot = new Cylinder
                {
                    Height = 20,
                    RTop = 10,
                    RBottom = 10,
                    Location = new Frame3D(150 - 25 + 10, 100 - 25 + 10, 3, Angle.Zero, Angle.Pi, Angle.Zero),
                    DefaultColor = Color.DarkViolet,
                    IsMaterial = true,
                    Density = Density.Iron,
                    FrictionCoefficient = 0,
                    Top = new PlaneImageBrush { Image = new Bitmap(GetResourceStream("blue.png")) },
                    NewId = Actor.ObjectId
                };
            }
            root.Add(robot);
        }

        private Stream GetResourceStream(string resourceName)
        {
            var assembly = GetType().Assembly;
            var names = assembly.GetManifestResourceNames();
            return assembly.GetManifestResourceStream("RepairTheStarship.KroR.Resources." + resourceName);
        }


        //public bool Release()
        //{
        //    var Body = (Actor.World.Engine as KroREngine).GetBody(Actor.ObjectId);
        //    var latestGripped = Body.FirstOrDefault(z => Actor.World.IdGenerator.KeyOfType<DetailColor>(z.NewId) );
            
        //    if (latestGripped == null) return false;
        //    var grippedColor = Actor.World.IdGenerator.GetKey<DetailColor>(latestGripped.NewId);

        //    var absoluteLocation = latestGripped.GetAbsoluteLocation();
        //    Body.Remove(latestGripped);

        //    latestGripped.FrictionCoefficient = frictionCoefficientsById.SafeGet(latestGripped.Id);
          
        //    latestGripped.Location = absoluteLocation;
        //    latestGripped.Velocity = Body.Velocity;
        //    var toAtt = Body.TreeRoot.GetSubtreeChildrenFirst()
        //                    .Where(a => Distance(latestGripped,a)<30)
        //                    .Where(a => Actor.World.IdGenerator.KeyOfType<WallData>(a.NewId))
        //                    .Where(a => Actor.World.IdGenerator.GetKey<WallData>(a.NewId).Match(grippedColor))
        //                    .OfType<Box>()
        //                    .FirstOrDefault();

            

        //    if (toAtt != null)
        //    {
        //        var oldWallData = Actor.World.IdGenerator.GetKey<WallData>(toAtt.NewId);
        //        Body.TreeRoot.Remove(toAtt);
        //        var wall = new Box
        //        {
        //            XSize = toAtt.XSize,
        //            YSize = toAtt.YSize,
        //            ZSize = toAtt.ZSize,
        //            Location = toAtt.Location,
        //            DefaultColor = RTSWorldManager.DefaultWallColor,
        //            IsStatic = true,
        //            IsMaterial = true,
        //            NewId = Actor.World.IdGenerator.CreateNewId(new WallData { Orientation=oldWallData.Orientation, Type= WallSettings.Wall })
        //        };
        //        Body.TreeRoot.Add(wall);
        //        return true;
        //    }
        
        //    Body.TreeRoot.Add(latestGripped);
        //    return false;
        //}


        //public string Grip()
        //{
        //    //TODO: тут была какая-то очень сложная логика, проверяющая, что происходит в случае, если тело уже захвачено. 
        //    //Сейчас это невозможно, и логику эту я выпилил. Проверить, ничего ли я не упустил
        //    var Body = (Actor.World.Engine as KroREngine).GetBody(Actor.ObjectId);
        //    var gripped = Body.ToList();
        //    if (gripped.Any())
        //        throw new Exception("Should not be here. The control to prevent the second gripping is on the logic side");

        //    var found = Body.TreeRoot.GetSubtreeChildrenFirst().FirstOrDefault(a => CanBeAttached(Body, a) && (a.Parent.NewId == a.TreeRoot.NewId));
        //    if (found == null) return null;
        //    CaptureDevicet(Body, found);
        //    return found.NewId;
            
        //}

        //private bool CanBeAttached(Body to, Body body)
        //{
        //    return body != to &&
        //        !body.IsStatic &&
        //        !to.SubtreeContainsChild(body) &&
        //        !to.ParentsContain(body) &&
        //        Actor.World.IdGenerator.KeyOfType<DetailColor>(body.NewId) &&
        //        Distance(body, to) < 30 && IsDetailAheadRobot(to.Location, body.Location);
        //}

        //private bool IsDetailAheadRobot(Frame3D robot, Frame3D detail)
        //{
        //    var angle = robot.Yaw.Grad;
        //    while (angle < 0)
        //        angle += 360;
        //    while (angle > 360)
        //        angle -= 360;
        //    const int angleLatitude = 40;
        //    const int detailDistance = 10;
        //    var detailAbove = detail.Y > robot.Y && Math.Abs(detail.Y - robot.Y) > detailDistance && angle >= 90 - angleLatitude && angle <= 90 + angleLatitude;
        //    var detailBelow = detail.Y < robot.Y && Math.Abs(detail.Y - robot.Y) > detailDistance && angle >= 270 - angleLatitude && angle <= 270 + angleLatitude;
        //    var detailLeft = detail.X < robot.X && Math.Abs(detail.X - robot.X) > detailDistance && angle >= 180 - angleLatitude && angle <= 180 + angleLatitude;
        //    var detailRight = detail.X > robot.X && Math.Abs(detail.X - robot.X) > detailDistance && ((angle >= 360 - angleLatitude && angle <= 360) || (angle >= 0 && angle <= angleLatitude));
        //    return detailAbove || detailBelow || detailLeft || detailRight;
        //}

        //private double Distance(Body from, Body to)
        //{
        //    return Geometry.Hypot(from.GetAbsoluteLocation() - to.GetAbsoluteLocation());
        //}

        //private readonly Dictionary<int, double> frictionCoefficientsById = new Dictionary<int, double>();

        //private void CaptureDevicet(Body box, Body newChild)
        //{

        //}






       
    }
}