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
    public class CollisionActorManager : ActorManager
    {

        

        public void Grip(string detailId)
        {
            var Engine = Actor.World.Engine as KroREngine;
            var box = Engine.GetBody(Actor.ObjectId);
            var newChild = Engine.GetBody(detailId);

            var childAbsolute = newChild.GetAbsoluteLocation();
            if (newChild.Parent != null)
                newChild.Parent.Remove(newChild);
            newChild.Location = box.GetAbsoluteLocation().Invert().Apply(childAbsolute);
            newChild.Location = newChild.Location.NewYaw(Angle.Zero);
            newChild.Location = newChild.Location.NewX(14);
            newChild.Location = newChild.Location.NewY(0);
            newChild.FrictionCoefficient = 0;
            box.Add(newChild);
        }
    }
}
