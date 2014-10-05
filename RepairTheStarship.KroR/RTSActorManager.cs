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
using RepairTheStarship.Robot;

namespace RepairTheStarship.KroR
{
    public class RTSActorManager : ActorManager<IRTSRobot>, IRTSActorManager
    {
        public string Grip()
        {
            return null;
        }

        public bool Release()
        {
            return false;
        }

        private Stream GetResourceStream(string resourceName)
        {
            var assembly = GetType().Assembly;
            var names = assembly.GetManifestResourceNames();
            return assembly.GetManifestResourceStream("RepairTheStarship.KroR.Resources." + resourceName);
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
    }
}