using System;
using System.Linq;

namespace ServerReplayPlayer.Contracts
{
    public enum Level
    {
        Level1,
        Level2
    }

    public static class LevelHelper
    {
        public static Level[] GetLevels()
        {
            return Enum.GetValues(typeof(Level)).Cast<Level>().ToArray();
        } 
    }
}
