using System;
using System.IO;
using log4net;

namespace ServerReplayPlayer.Logic
{
    public static class Logger
    {
        private static readonly ILog logger;
        private static readonly string LogFileConfig = Helpers.GetServerPath("settings\\log4netConfig.xml");

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
    }
}