using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileAnalyzer.Exceptions;

namespace FileAnalyzer.Readers
{
    public class FileReaderFactory
    {
        private readonly List<IFileReader> _readers = new List<IFileReader>
        {
            new TxtFileReader(),
            new DocxFileReader(),
            new PdfFileReader()
        };

        public IEnumerable<string> SupportedExtensions =>
            _readers.Select(reader => reader.SupportedExtension);

        public IFileReader GetReader(string filePath)
        {
            string extension = Path.GetExtension(filePath);

            IFileReader reader = _readers.FirstOrDefault(r =>
                string.Equals(r.SupportedExtension, extension, StringComparison.OrdinalIgnoreCase));

            if (reader == null)
            {
                throw new UnsupportedFileTypeException(extension);
            }

            return reader;
        }
    }
}