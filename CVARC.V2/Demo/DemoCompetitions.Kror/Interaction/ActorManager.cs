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
    public class IntActorManager : ActorManager<IActor>
    {


        private Stream GetResourceStream(string resourceName)
        {
            var assembly = GetType().Assembly;
            var names = assembly.GetManifestResourceNames();
            return assembly.GetManifestResourceStream("Demo.KroR.Resourses." + resourceName);
        }

        public override void CreateActorBody()
        {
            var root = (Actor.World.Engine as KroREngine).Root;

            string fileName = "red.png";

            if ((Actor.World as MovementWorld).WorldState.RectangularRobot)
            {
                var box = new Box
                {
                    XSize = 20,
                    YSize = 20,
                    ZSize = 20,
                    DefaultColor = Color.Violet,
                    IsMaterial = true,
                    NewId = Actor.ObjectId,
                    Location = new Frame3D(0, 0, 3, Angle.Zero, Angle.Zero, Angle.Zero),
                    Density = Density.Iron,
                    FrictionCoefficient = 0,
                    Top = new PlaneImageBrush { Image = Bitmap.FromStream(GetResourceStream(fileName)) },
                };
                root.Add(box);
            }
            else
            {
                var cyllinder = new Cylinder
                {

                    Height = 20,
                    RTop = 10,
                    RBottom = 10,
                    Location = new Frame3D(0, 0, 3, Angle.Zero, Angle.Zero, Angle.Zero),
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
}
