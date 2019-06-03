using System;

namespace Pos.Contracts
{
    public interface ILoggerHelper
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message);
        void LogError(Exception e, string message = "");
    }
}
