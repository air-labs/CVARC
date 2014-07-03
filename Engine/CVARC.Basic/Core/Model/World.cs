using System.Collections.Generic;
using System.Linq;
using CVARC.Core;
using CVARC.Graphics;
using CVARC.Network;

namespace CVARC.Basic
{
    public abstract class World
    {
        public IEngine Engine { get; private set; }
        public ScoreCollection Score { get; private set; }
        public ISceneSettings Settings { get; private set; }
        public HelloPackage HelloPackage { get; set; }
      
        public void Init(IEngine engine)
        {
            Engine = engine;
            Settings = ParseSettings(HelloPackage);
            Robots = Enumerable.Range(0, RobotCount).Select(CreateRobot).ToList();
            Score = new ScoreCollection(RobotCount);
            Engine.Initialize(Settings);
            Robots.ForEach(x => x.Init());
        }

        public List<Robot> Robots { get; private set; }

        public virtual int RobotCount { get { return 2; } }

        public virtual int CompetitionId { get { return 1; } }

        public abstract ISceneSettings ParseSettings(HelloPackage helloPackage);

        public abstract Robot CreateRobot(int robotNumber);
    }
}
