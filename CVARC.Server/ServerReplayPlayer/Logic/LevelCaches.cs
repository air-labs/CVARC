using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    class LevelCaches
    {
        public FileCache<MatchResultEntity> MatchResultCache { get; set; } 
        public FileCache<PlayerEntity> PlayerCache { get; set; }

        public LevelCaches(string folder)
        {
            PlayerCache = new FileCache<PlayerEntity>(folder + "_players");
            MatchResultCache = new FileCache<MatchResultEntity>(folder + "_result");
        }
    }
}