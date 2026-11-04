using System;

namespace FileAnalyzer.Exceptions
{
    public class FileReadException : Exception
    {
        public string FilePath { get; }

        public FileReadException(string filePath, Exception innerException)
            : base($"The file could not be read: {filePath}", innerException)
        {
            FilePath = filePath;
        }
    }
}