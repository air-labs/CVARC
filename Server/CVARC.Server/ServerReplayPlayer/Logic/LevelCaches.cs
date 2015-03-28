using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Logic
{
    class LevelCaches
    {
        public IFileCache<MatchResultEntity> MatchResultCache { get; set; }
        public IFileCache<PlayerEntity> PlayerCache { get; set; }

        public LevelCaches(string folder)
        {
            PlayerCache = new FileCache<PlayerEntity>(folder + "_players", false);
            MatchResultCache = new FileCache<MatchResultEntity>(folder + "_result", true);
        }
    }
}