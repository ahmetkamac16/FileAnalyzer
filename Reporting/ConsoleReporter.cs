using FileAnalyzer.Models;
using System;
using System.Linq;

namespace FileAnalyzer.Reporting
{
    public class ConsoleReporter
    {
        private const string Separator = "----------------------------------------";

        public void Report(AnalysisResult result)
        {
            PrintHeader(result);
            PrintWordSummary(result);
            PrintRepeatedWords(result);
            PrintPunctuation(result);
        }

        private void PrintHeader(AnalysisResult result)
        {
            Console.WriteLine(Separator);
            Console.WriteLine("File: " + result.FilePath);
            Console.WriteLine(Separator);
        }

        private void PrintWordSummary(AnalysisResult result) 
        {
            Console.WriteLine();
            Console.WriteLine("Word Summary (numbers and conjunctions excluded)");
            Console.WriteLine("  Analyzed words: " + result.TotalWordCount);
            Console.WriteLine("  Distinct words: " + result.DistinctWordCount);
        }

        private void PrintRepeatedWords(AnalysisResult result) 
        {
            var repeatedWords = result.WordFrequencies
                .Where(pair => pair.Value > 1)
                .ToList();

            Console.WriteLine();
            Console.WriteLine("Repeated Words");

            if (repeatedWords.Count == 0)
            {
                Console.WriteLine("  No repeated words found.");
                return;
            }

            foreach (var pair in repeatedWords) 
            {
                Console.WriteLine($"  {pair.Key,-20} {pair.Value,5}");
            }
        }

        private void PrintPunctuation(AnalysisResult result) 
        {
            Console.WriteLine();
            Console.WriteLine("Punctuation Marks");

            if (result.PunctuationCounts.Count == 0)
            {
                Console.WriteLine("  No punctuation marks found.");
                return;
            }

            foreach (var pair in result.PunctuationCounts)
            {
                Console.WriteLine($"  '{pair.Key}' {pair.Value,5}");
            }
        }
    }
}
