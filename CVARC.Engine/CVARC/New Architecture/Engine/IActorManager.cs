using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;

namespace CVARC.V2
{
    public interface IActorManager
    {
        void Initialize(IActor actor);
        IActor Actor { get; }
        void CreateActorBody();
    }

    public static class IActorManagerExtensions
    {
        public static Frame3D GetAbsoluteLocation(this IActorManager actorManager)
        {
            return actorManager.Actor.World.Engine.GetAbsoluteLocation(actorManager.Actor.ObjectId);
        }
        public static void SetSpeed(this IActorManager actorManager, Frame3D speed)
        {
            actorManager.Actor.World.Engine.SetSpeed(actorManager.Actor.ObjectId, speed);
        }
    }

   
        
}
