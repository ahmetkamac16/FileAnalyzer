namespace FileAnalyzer.Readers
{
    public interface IFileReader
    {
        string SupportedExtension { get; }
        string ReadText(string filePath);
    }
}
