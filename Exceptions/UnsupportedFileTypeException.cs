using System;

namespace FileAnalyzer.Exceptions
{
    public class UnsupportedFileTypeException : Exception
    {
        public string Extension { get; }

        public UnsupportedFileTypeException(string extension)
            : base($"Files with the '{extension}' extension are not supported.")
        {
            Extension = extension;
        }
    }
}