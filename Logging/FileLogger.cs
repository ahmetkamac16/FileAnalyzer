using System;
using System.IO;

namespace FileAnalyzer.Logging
{
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;

        public FileLogger()
        {
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(logDirectory);

            string fileName = $"log_{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
            _logFilePath = Path.Combine(logDirectory, fileName);
        }

        public void Info(string message)
        {
            Write("INFO", message);
        }

        public void Error(string message, Exception exception)
        {
            Write("ERROR", $"{message} | {exception.GetType().Name}: {exception.Message}");
        }

        private void Write(string level, string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string line = $"[{timestamp}] [{level}] {message}";

            try
            {
                File.AppendAllText(_logFilePath, line + Environment.NewLine);
            }
            catch
            {
                // Logging must never crash the application.
                // If the log file cannot be written (disk full, no permission), the error is ignored.
            }
        }
    }
}