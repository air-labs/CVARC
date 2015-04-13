namespace ServerReplayPlayer.Contracts
{
    public class LevelResult
    {
        public string LevelName { get; set; }
        public int Points { get; set; }

        public LevelResult(string levelName, int points)
        {
            LevelName = levelName;
            Points = points;
        }
    }
}