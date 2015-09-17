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
    public class ActorManager : ActorManager<IActor>
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
			var state = (Actor.World.WorldState) as DemoWorldState;

			var description = state.Robots.Where(z => z.RobotName == Actor.ControllerId).FirstOrDefault();


            if (description == null)
            {
                root.Add(
                new Cylinder
					{
						Height = 10,
						RTop = 10,
						RBottom = 10,
						Location = new Frame3D(300,300,5),
						DefaultColor = Color.White,
						IsMaterial = false,
						Density = Density.Iron,
						FrictionCoefficient = 0,
						NewId = Actor.ObjectId
					});
                return;
            }
            string fileName = "red.png";
			if (Actor.ControllerId == TwoPlayersId.Right) fileName = "blue.png";

			var location = new Frame3D(description.X, description.Y, description.ZSize / 2, Angle.Zero,description.Yaw, Angle.Zero);

			if (description.IsRound)
				root.Add(new Cylinder
					{
						Height = description.ZSize,
						RTop = description.XSize,
						RBottom = description.XSize,
						Location = location,
						DefaultColor = MovementWorldManager.ToColor(description.Color),
						IsMaterial = true,
						Density = Density.Iron,
						FrictionCoefficient = 0,
						Top = new PlaneImageBrush { Image = Bitmap.FromStream(GetResourceStream(fileName)) },
						NewId = Actor.ObjectId
					});
			else
				root.Add(new Box
				{
					XSize = description.XSize,
					YSize = description.YSize,
					ZSize = description.ZSize,
					DefaultColor = MovementWorldManager.ToColor(description.Color),
					IsMaterial = true,
					NewId = Actor.ObjectId,
					Location = location,
					Density = Density.Iron,
					Top = new PlaneImageBrush { Image = Bitmap.FromStream(GetResourceStream(fileName)) },
				});
        }
    }
}
