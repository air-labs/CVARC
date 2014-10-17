using AIRLab.Mathematics;
using CVARC.Core;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jango
{
    public enum Parts
    {
        BaseJoint,
        BaseBone
    };

    public class JangoActorManager : ActorManager<JangoRobot>
    {
        public override void CreateActorBody()
        {
            var root = ((KroREngine)Actor.World.Engine).Root;

            var generator = Actor.World.IdGenerator;

            var joint = new Ball
            {
                Radius = 10,
                NewId = generator.CreateNewId(Parts.BaseJoint),
                Location = new Frame3D(0, 0, 0),
                DefaultColor = Color.Red
            };

            root.Add(joint);

            var bone = new Box
            {
                NewId = generator.CreateNewId(Parts.BaseBone),
                XSize = 100,
                YSize = 15,
                ZSize = 15,
                DefaultColor = Color.Green,
                Location = new Frame3D(-50, 0, 0)
            };
            joint.Add(bone);
        }
    }
}
