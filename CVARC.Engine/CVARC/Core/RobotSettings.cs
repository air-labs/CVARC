namespace CVARC.Basic
{
    public class RobotSettings
    {
        public RobotSettings(int number, bool isBot)
        {
            Number = number;
            IsBot = isBot;
        }

        public int Number { get; set; }
        public bool IsBot { get; set; }
    }
}