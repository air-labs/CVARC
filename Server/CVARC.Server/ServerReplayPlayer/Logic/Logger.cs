using System;
using System.IO;
using ServerReplayPlayer.Logic.Providers;
using log4net;

namespace ServerReplayPlayer.Logic
{
    public static class Logger
    {
        private static readonly ILog logger;
        private static readonly string LogFileConfig = SettingsProvider.GetSettingsFilePath("log4netConfig.xml");

        static Logger()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(LogFileConfig));
            logger = LogManager.GetLogger(typeof(Logger));
        }

        public static void Error(Exception e)
        {
            logger.Error(e);
        }

        public static void Info(string message)
        {
            logger.Info(message);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            logger.InfoFormat(format, args);
        }
    }
}