using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ServerReplayPlayer.Logic.Providers
{
    public class DeadlineProvider
    {
        private static Dictionary<string, DateTime> deadlines;
        private static readonly string DeadlinesFilePath = SettingsProvider.GetSettingsFilePath("deadlines.txt");

        private static Dictionary<string, DateTime> GetDeadlinesByLevelName()
        {
            if (deadlines == null)
            {
                var tempDeadlines = File.ReadAllLines(DeadlinesFilePath).Select(x =>
                    {
                        var splits = x.Split('=');
                        return new {LevelName = splits[0], Date = DateTime.Parse(splits[1])};
                    }).ToDictionary(x => x.LevelName, x => x.Date);
                deadlines = tempDeadlines;
            }
            return deadlines;
        }

        public static bool CanUploadClient(string levelName)
        {
            return DateTime.UtcNow <= GetDeadlinesByLevelName()[levelName];
        }

        public static void ChangeDeadlines(HttpPostedFileBase file)
        {
            file.SaveAs(DeadlinesFilePath);
            deadlines = null;
        }
    }
}