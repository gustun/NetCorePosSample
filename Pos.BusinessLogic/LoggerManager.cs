using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using Pos.Contracts;

namespace Pos.BusinessLogic
{
    public class LoggerManager : ILoggerManager
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public LoggerManager()
        {
        }

        public void LogDebug(string message)
        {
            Logger.Debug(message);
        }

        public void LogError(string message)
        {
            Logger.Error(message);
        }

        public void LogError(Exception e, string message = "")
        {
            message += $" Exception: {e.Message}, Stacktrace: {e.StackTrace}";
            LogError(message);
        }

        public void LogInfo(string message)
        {
            Logger.Info(message);
        }

        public void LogWarn(string message)
        {
            Logger.Warn(message);
        }
    }
}
