using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    class LevelCaches
    {
        public FileCache<MatchResult> MatchResultCache { get; set; } 
        public FileCache<Player> PlayerCache { get; set; }

        public LevelCaches(string folder)
        {
            PlayerCache = new FileCache<Player>(folder + "_players");
            MatchResultCache = new FileCache<MatchResult>(folder + "_result");
        }
    }
}