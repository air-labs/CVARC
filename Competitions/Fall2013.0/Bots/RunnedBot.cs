using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;
using Gems.Sensors;

namespace StarshipRepair.Bots
{
    public class RunnedBot : FixedProgramBot
    {
        private int lookAt;
        protected Node currentPosition;
        protected Map map;

        public override void DefineProgram()
        {
            Init(Competitions);
        }
        protected void Init(Competitions competitions)
        {
            if (map == null)
            {
                try
                {
                    map = (competitions.Settings  as SceneSettings).Map;
                    lookAt = -180*ControlledRobot;
                    currentPosition = map[5*ControlledRobot, 0];
                }
                catch (Exception e)
                {
                    
                }
            }
        }
        public void GoTo(Node to)
        {
            var pth = map.FindPath(currentPosition.X, currentPosition.Y, to.X, to.Y);
            foreach (var next in pth)
            {
                if (next == currentPosition) continue;
                var needLookAt = (int) (Math.Atan2(next.Y - currentPosition.Y, next.X - currentPosition.X)/Math.PI*180);
                if (needLookAt != lookAt)
                {
                    if (Math.Abs(needLookAt - lookAt) <= 180)
                        Rot((-1+2*ControlledRobot)*(needLookAt - lookAt));
                    else
                        Rot((-1 + 2 * ControlledRobot) * (-(360 - needLookAt + lookAt)));
                }
                lookAt = needLookAt;
                Mov(50);
                currentPosition = next;
            }
        }
    }
}
