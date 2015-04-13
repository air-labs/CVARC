using System.Collections.Generic;

namespace ServerReplayPlayer.Contracts
{
    public class TeamResult
    {
        public string TeamName { get; set; }
        public Dictionary<string, int> PointByLevel { get; set; }

        public TeamResult(string teamName, Dictionary<string, int> pointByLevel)
        {
            TeamName = teamName;
            PointByLevel = pointByLevel;
        }
    }
}