using System;
using System.Linq;
using UglyToad.PdfPig;

namespace FileAnalyzer.Readers
{
    public class PdfFileReader : IFileReader
    {
        public string SupportedExtension => ".pdf";

        public string ReadText(string filePath)
        {
            using (var document = PdfDocument.Open(filePath))
            {
                var pages = document.GetPages()
                    .Select(page => string.Join(" ", page.GetWords().Select(word => word.Text)));

                return string.Join(Environment.NewLine, pages);
            }
        }
    }
}
