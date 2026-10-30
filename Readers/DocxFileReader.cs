using System;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace FileAnalyzer.Readers
{
    public class DocxFileReader : IFileReader
    {
        public string SupportedExtension => ".docx";

        public string ReadText(string filePath)
        {
            using (var document = WordprocessingDocument.Open(filePath, false))
            {
                Body body = document.MainDocumentPart.Document.Body;

                var paragraphs = body.Descendants<Paragraph>().Select(p => p.InnerText);
                return string.Join(Environment.NewLine, paragraphs);
            }
        }
    }
}