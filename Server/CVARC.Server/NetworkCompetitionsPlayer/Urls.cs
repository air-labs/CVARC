using System.Configuration;

namespace NetworkCompetitionsPlayer
{
    public static class Urls
    {
        private static readonly string Port = ConfigurationManager.AppSettings["portNumber"];
        private static readonly string Url = string.Format("http://localhost:{0}/Replay/", Port);
        public static readonly string GetPlayer = Url + "GetPlayer";
        public static string SaveMatchResult = Url + "SaveMatchResult";
        public static readonly string GetCompetitionsInfo = Url + "GetCompetitionsInfo";
    }
}
