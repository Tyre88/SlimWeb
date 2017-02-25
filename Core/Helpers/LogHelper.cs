using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace SW.Core.Helpers
{
    public class LogHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void LogError(string message, Exception ex, int clubId, params object[] obj)
        {
            LogManager.Configuration.Variables["clubId"] = clubId.ToString();
            logger.Log(LogLevel.Error, ex, message, obj);
        }

        public static void LogDebug(string message, Exception ex, int clubId, params object[] obj)
        {
            LogManager.Configuration.Variables["clubId"] = clubId.ToString();
            logger.Log(LogLevel.Debug, ex, message, obj);
        }

        public static void LogInfo(string message, int clubId, Exception ex = null, params object[] obj)
        {
            LogManager.Configuration.Variables["clubId"] = clubId.ToString();
            logger.Log(LogLevel.Info, ex, message, obj);
        }

        public static void LogFatal(string message, Exception ex, int clubId, params object[] obj)
        {
            LogManager.Configuration.Variables["clubId"] = clubId.ToString();
            logger.Log(LogLevel.Fatal, ex, message, obj);
        }

        public static void LogWarn(string message, Exception ex, int clubId, params object[] obj)
        {
            LogManager.Configuration.Variables["clubId"] = clubId.ToString();
            logger.Log(LogLevel.Warn, ex, message, obj);
        }
    }
}
