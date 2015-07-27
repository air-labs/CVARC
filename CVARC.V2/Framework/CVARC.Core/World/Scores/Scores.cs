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
            this.world = world;
            Records = new Dictionary<string, List<ScoreRecord>>();
        }

        public Dictionary<string, List<ScoreRecord>> Records { get; private set; } 
        public event Action ScoresChanged;
        
        public void Add(string controllerId, int count, string reason, RecordType type=RecordType.Permanent)
        {
            if (!Records.ContainsKey(controllerId))
                Records[controllerId] = new List<ScoreRecord>();
            Records[controllerId].Add(new ScoreRecord(count, reason, world.Clocks.CurrentTime, type));
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

        public void DeleteTemporaryRecords()
        {
            Records = Records
                .ToDictionary(kv => kv.Key, kv => kv.Value
                    .Where(r => r.Type == RecordType.Permanent)
                    .ToList());
            if (ScoresChanged != null) ScoresChanged();
        }
    }
}
