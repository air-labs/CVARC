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

namespace Demo
{
    public class CameraActorManager : ActorManager<CameraRobot>
    {


        private Stream GetResourceStream(string resourceName)
        {
            var assembly = GetType().Assembly;
            var names = assembly.GetManifestResourceNames();
            return assembly.GetManifestResourceStream("Demo.KroR.Resourses."+resourceName);
        }

        public override void CreateActorBody()
        {
            var root=(Actor.World.Engine as KroREngine).Root;

            double X = -50;
            string fileName = "red.png";

            if (Actor.ControllerId == TwoPlayersId.Right)
            {
                X = 50;
                fileName = "blue.png";
            }

            var cyllinder = new Cylinder
            {
                
                Height = 20,
                RTop = 10,
                RBottom = 10,
                Location = new Frame3D(X, 50, 3,Angle.Zero,-Angle.Pi/2,Angle.Zero),
                DefaultColor = Color.DarkViolet,
                IsMaterial = true,
                Density = Density.Iron,
                FrictionCoefficient = 0,
                Top = new PlaneImageBrush { Image = Bitmap.FromStream(GetResourceStream(fileName)) },
                NewId = Actor.ObjectId
            };
            root.Add(cyllinder);
        }
    }
}
