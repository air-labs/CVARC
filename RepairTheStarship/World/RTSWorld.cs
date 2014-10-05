using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;
using RepairTheStarship.Robot;

namespace RepairTheStarship
{
    public class RTSWorld : World<SceneSettings,IRTSWorldManager>, ISimpleMovementWorld
    {
        public void DetailInstalled(DetailColor color, string robotId)
        {
            //TODO: реализовать логику начисления очков
        }

        protected override IEnumerable<IActor> CreateActors()
        {
            yield return new Level1Robot(TwoPlayersId.Left);
            yield return new Level1Robot(TwoPlayersId.Right);
        }

        public double LinearVelocityLimit
        {
            get { return 50; }
        }

        public AIRLab.Mathematics.Angle AngularVelocityLimit
        {
            get { return Angle.FromGrad(50); }
        }
    }
}
