using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FileAnalyzer.Analysis;
using FileAnalyzer.Exceptions;
using FileAnalyzer.Logging;
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

            ILogger logger = new FileLogger();
            logger.Info("Application started.");

            try
            {
                Run(logger);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                logger.Error("Unexpected error occurred.", ex);
            }
            finally
            {
                logger.Info("Application finished.");
                WaitForExit();
            }
        }

        private static void Run(ILogger logger)
        {
            var factory = new FileReaderFactory();

            string filePath = SelectFile(factory);
            if (filePath == null)
            {
                Console.WriteLine("No file selected.");
                logger.Info("File selection cancelled by user.");
                return;
            }

            logger.Info($"File selected: {filePath}");

            try
            {
                IFileReader reader = factory.GetReader(filePath);
                logger.Info($"Reader selected: {reader.GetType().Name}");

                string text = ReadFile(reader, filePath);
                logger.Info($"File read successfully ({text.Length} characters).");

                var result = new AnalysisResult { FilePath = filePath };
                new TextAnalyzer().Analyze(text, result);
                new PunctuationAnalyzer().Analyze(text, result);

                int punctuationTotal = result.PunctuationCounts.Sum(pair => pair.Value);
                logger.Info($"Analysis completed: {result.TotalWordCount} words, " + $"{result.DistinctWordCount} distinct, {punctuationTotal} punctuation marks.");

                new ConsoleReporter().Report(result);
            }
            catch (UnsupportedFileTypeException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Supported types: {string.Join(", ", factory.SupportedExtensions)}");
                logger.Error("Unsupported file type selected.", ex);
            }
            catch (FileReadException ex)
            { 
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Reason: {ex.InnerException.Message}");
                }
                logger.Error(ex.Message, ex.InnerException ?? ex);
            }
        }

        private static string ReadFile(IFileReader reader, string filePath)
        {
            try
            {
                return reader.ReadText(filePath);
            }
            catch (Exception ex)
            {
                throw new FileReadException(filePath, ex);
            }
        }

        private static string SelectFile(FileReaderFactory factory)
        {
            string patterns = string.Join(";", factory.SupportedExtensions.Select(ext => "*" + ext));

            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Select a file to analyze";
                dialog.Filter = $"Supported files ({patterns})|{patterns}|All files (*.*)|*.*";

                return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName: null;
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