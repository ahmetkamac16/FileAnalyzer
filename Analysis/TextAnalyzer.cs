using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using FileAnalyzer.Models;

namespace FileAnalyzer.Analysis
{
    public class TextAnalyzer
    {
        private static readonly Regex WordPattern =
            new Regex(@"[\p{L}\p{N}]+(?:['’]\p{L}+)?", RegexOptions.Compiled);

        private static readonly CultureInfo TurkishCulture = new CultureInfo("tr-TR");

        public void Analyze(string text, AnalysisResult result)
        {
            var frequencies = new Dictionary<string, int>();

            foreach (Match match in WordPattern.Matches(text))
            {
                string word = NormalizeWord(match.Value);

                if (IsNumber(word) || StopWords.Contains(word))
                {
                    continue;
                }

                if (frequencies.ContainsKey(word))
                {
                    frequencies[word]++;
                }
                else
                {
                    frequencies[word] = 1;
                }
            }

            result.TotalWordCount = frequencies.Values.Sum();
            result.DistinctWordCount = frequencies.Count;
            result.WordFrequencies = frequencies
                .OrderByDescending(pair => pair.Value)
                .ThenBy(pair => pair.Key, StringComparer.Create(TurkishCulture, false))
                .ToList();
        }

        private static string NormalizeWord(string rawWord)
        {
            int apostropheIndex = rawWord.IndexOfAny(new[] { '\'', '’' });
            string root = apostropheIndex >= 0 ? rawWord.Substring(0, apostropheIndex) : rawWord;

            return root.ToLower(TurkishCulture);
        }

        private static bool IsNumber(string word) => word.All(char.IsDigit);
    }
}