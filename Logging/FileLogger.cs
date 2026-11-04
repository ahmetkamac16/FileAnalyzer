using System;
using System.IO;

namespace FileAnalyzer.Logging
{
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;

        public FileLogger()
        {
            string logDirectory = ResolveLogDirectory();
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

        private static string ResolveLogDirectory()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                var directory = new DirectoryInfo(baseDirectory);

                while (directory != null)
                {
                    if (directory.GetFiles("*.csproj").Length > 0)
                    {
                        return Path.Combine(directory.FullName, "Logs");
                    }

                    directory = directory.Parent;
                }
            }
            catch
            {
                // If the folder tree cannot be searched, fall back to the executable's folder.
            }

            return Path.Combine(baseDirectory, "Logs");
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