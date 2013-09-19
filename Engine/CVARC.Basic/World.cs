using System.Collections.Generic;
using System.Linq;
using CVARC.Core;

namespace CVARC.Basic
{
    public abstract class World
    {
        public ScoreCollection Score { get; private set; }
        public Body Init()
        {
            Robots = Enumerable.Range(0, RobotCount).Select(a => new Robot(this) {Number = a}).ToList();
            Score = new ScoreCollection(RobotCount);
            var root = CreateWorld(Robots);
            return root;
        }

        public List<Robot> Robots { get; private set; }

        public virtual int RobotCount { get { return 2; } }

        public virtual int CompetitionId { get { return 1; } }

        public abstract Body CreateWorld(IEnumerable<Robot> robots);
    }
}
