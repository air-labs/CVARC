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

namespace DemoCompetitions
{
    class DemoActorManager : ActorManager<DemoRobot>, IDemoActorManagerPrototype
    {
        public void MakeAction()
        {
           
        }


        private Stream GetResourceStream(string resourceName)
        {
            return GetType().Assembly.GetManifestResourceStream("Gems.Resources." + resourceName);
        }

        public override void CreateActorBody()
        {
            var root=(Actor.World.Engine as KRPhysical).Root;
            var cyllinder = new Cylinder
            {
                
                Height = 20,
                RTop = 10,
                RBottom = 10,
                Location = new Frame3D(-50, 50, 3),
                DefaultColor = Color.DarkViolet,
                IsMaterial = true,
                Density = Density.Iron,
                FrictionCoefficient = 0,
                Top = new PlaneImageBrush { Image = Bitmap.FromFile("red.png") },
                Type = "Robot",
                NewId = Actor.ObjectId
                
            };
            root.Add(cyllinder);
        }
    }
}
