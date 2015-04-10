using System.Configuration;

namespace NetworkCompetitionsPlayer
{
    public static class Urls
    {
        private static readonly string Url = ConfigurationManager.AppSettings["hostName"];
        public static readonly string GetPlayer = Url + "Replay/GetPlayer";
        public static readonly string SaveMatchResult = Url + "Replay/SaveMatchResult";
        public static readonly string GetCompetitionsInfo = Url + "Replay/GetCompetitionsInfo";
    }
}
