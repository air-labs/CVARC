using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIRLab.Mathematics;
using CVARC.Core;
using CVARC.V2;


namespace RoboMovies.KroR
{
    public class RMActorManager : ActorManager<IActor>, IRMActorManager
    {

        public void EffectOnCapture(string detailId)
        {
            
        }

        public void EffectOnUnsuccessfullRelease(string detailId)
        {
        }

        public override void CreateActorBody()
        {
            var location = new Frame3D();
            Bitmap topTexture = null;

            if (Actor.ControllerId == TwoPlayersId.Left)
            {
                location = new Frame3D(-150 + 35, 0, 3);
                topTexture = new Bitmap(WorldInitializerHelper.GetResourceStream("red.png"));
            }
            else
            {
                location = new Frame3D(150 - 35, 0, 3, Angle.Zero, Angle.Pi, Angle.Zero);
                topTexture = new Bitmap(WorldInitializerHelper.GetResourceStream("blue.png"));
            }

            var root = (Actor.World.Engine as KroREngine).Root;
            
            root.Add(new Cylinder
            {
                Height = 20,
                RTop = 12,
                RBottom = 12,
                Location = location,
                DefaultColor = Color.DarkViolet,
                IsMaterial = true,
                Density = Density.Iron,
                FrictionCoefficient = 0,
                Top = new PlaneImageBrush { Image = topTexture }, 
                NewId = Actor.ObjectId
            });
        }
    }
}