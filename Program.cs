using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FileAnalyzer.Analysis;
using FileAnalyzer.Exceptions;
using FileAnalyzer.Models;
using FileAnalyzer.Readers;
using FileAnalyzer.Reporting;


namespace FileAnalyzer
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var factory = new FileReaderFactory();

            string filePath = SelectFile(factory);
            if (filePath == null)
            {
                Console.WriteLine("No file selected.");
                WaitForExit();
                return;
            }

            try
            {
                IFileReader reader = factory.GetReader(filePath);
                string text = reader.ReadText(filePath);

                var result = new AnalysisResult { FilePath = filePath };
                new TextAnalyzer().Analyze(text, result);
                new PunctuationAnalyzer().Analyze(text, result);

                new ConsoleReporter().Report(result);
            }
            catch (UnsupportedFileTypeException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            WaitForExit();
        }

        private static string SelectFile(FileReaderFactory factory)
        {
            string patterns = string.Join(";", factory.SupportedExtensions.Select(ext => "*" + ext));

            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Select a file to analyze";
                dialog.Filter = $"Supported files ({patterns})|{patterns}|All files (*.*)|*.*";

                return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
            }
        }

        private static void WaitForExit()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}