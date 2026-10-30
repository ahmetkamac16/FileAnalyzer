using System.Collections.Generic;


namespace FileAnalyzer.Models
{
    public class AnalysisResult
    {
        public string FilePath { get; set; }
        public int TotalWordCount { get; set; }
        public int DistinctWordCount { get; set; }
        public List<KeyValuePair<string, int>> WordFrequencies { get; set; } = new List<KeyValuePair<string, int>>();
        public List<KeyValuePair<char, int>> PunctuationCounts { get; set; } = new List<KeyValuePair<char, int>>();
    }
}
