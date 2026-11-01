using System.Collections.Generic;
using System.Linq;
using FileAnalyzer.Models;

namespace FileAnalyzer.Analysis
{
    public class PunctuationAnalyzer
    {
        public void Analyze(string text, AnalysisResult result)
        {
            var counts = new Dictionary<char, int>();

            foreach (char c in text)
            {
                if (!char.IsPunctuation(c)) continue;


                if (counts.ContainsKey(c))
                {
                    counts[c]++;
                }
                else
                    counts[c] = 1;
            }

            result.PunctuationCounts = counts
                .OrderByDescending(x => x.Value)
                .ThenBy(x => x.Key)
                .ToList();
        }
    }
}
