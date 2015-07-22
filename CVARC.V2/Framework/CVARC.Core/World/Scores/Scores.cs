using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab;

namespace CVARC.V2
{
    public class Scores
    {
        IWorld world;
        public Scores(IWorld world)
        {
            this.world=world;
        }
        public readonly Dictionary<string, List<ScoreRecord>> Records = new Dictionary<string, List<ScoreRecord>>();
        public event Action ScoresChanged;
        public void Add(string controllerId, int count, string reason)
        {
            if (!Records.ContainsKey(controllerId))
                Records[controllerId] = new List<ScoreRecord>();
            Records[controllerId].Add(new ScoreRecord(count, reason, world.Clocks.CurrentTime));
            if (ScoresChanged != null) ScoresChanged();
        }
        public IEnumerable<Tuple<string, int>> GetAllScores()
        {
            return Records.Keys.Select(z => new Tuple<string, int>(z, Records[z].Sum(x => x.Count)));
        }
        public int GetTotalScore(string controllerId)
        {
            if (!Records.ContainsKey(controllerId)) 
                throw new ArgumentException("Unrecognized controller ID.");
            return Records[controllerId].Sum(x => x.Count);
        }
    }
}
