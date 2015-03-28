namespace ServerReplayPlayer.Contracts
{
    public class CommandEntity
    {
        public string CommandName { get; set; }
        public string Captain { get; set; }
        public string Email { get; set; }
        public string[] Participants { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
