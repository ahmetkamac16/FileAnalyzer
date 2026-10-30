using System.Collections.Generic;

namespace FileAnalyzer.Analysis
{
    public static class StopWords
    {
        private static readonly HashSet<string> _words = new HashSet<string>()
        {
            "ve", "ile", "ama", "ancak", "fakat", "lakin", "veya", "yahut", "veyahut",
            "ya", "yani", "de", "da", "ki", "çünkü", "hem", "ne", "ise", "oysa",
            "oysaki", "halbuki", "eğer", "şayet", "madem", "mademki", "yoksa",
            "üstelik", "ayrıca", "hatta", "bile", "dahi", "zira", "nitekim", "gerek", "ister"
        };
        public static bool Contains(string word) => _words.Contains(word);
    }

}
