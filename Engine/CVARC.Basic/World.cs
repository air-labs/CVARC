using System.Collections.Generic;
using System.Linq;
using CVARC.Core;
using CVARC.Network;

namespace CVARC.Basic
{
    public abstract class World
    {
        public HelloPackage HelloPackage = new HelloPackage();
        public ScoreCollection Score { get; private set; }
        public Body Init()
        {
            Robots = Enumerable.Range(0, RobotCount).Select(a => CreateRobot(a)).ToList();
            Score = new ScoreCollection(RobotCount);
            var root = CreateWorld(Robots);
            return root;
        }

        public List<Robot> Robots { get; private set; }

        public virtual int RobotCount { get { return 2; } }

        public virtual int CompetitionId { get { return 1; } }

        public abstract Body CreateWorld(IEnumerable<Robot> robots);

        public abstract Robot CreateRobot(int robotNumber);
    }
}
