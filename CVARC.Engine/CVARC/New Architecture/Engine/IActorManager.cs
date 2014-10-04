using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;

namespace CVARC.V2
{
    public interface IActorManager
    {
        IActor Actor { get; }
        void CreateActorBody(IActor actor, IdGenerator generator);
    }

    public static class IActorManagerExtensions
    {
        public static Frame3D GetAbsoluteLocation(this IActorManager actorManager)
        {
            return actorManager.Actor.WorldAsIWorld.Engine.Physical.GetAbsoluteLocation(actorManager.Actor.ObjectId);
        }
        public static void SetSpeed(this IActorManager actorManager, Frame3D speed)
        {
            actorManager.Actor.WorldAsIWorld.Engine.Physical.SetSpeed(actorManager.Actor.ObjectId, speed);
        }
    }
}
