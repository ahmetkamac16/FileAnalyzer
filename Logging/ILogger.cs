using System;

namespace FileAnalyzer.Logging
{
    public interface ILogger
    {
        void Info(string message);
        void Error(string message, Exception exception);
    }
}
