# FileAnalyzer

## Project Overview

FileAnalyzer is a C# console application that extracts text from `.txt`, `.docx`, and `.pdf` files and reports word and punctuation statistics. Each run opens a Windows file picker for selecting one file. A reader extracts the text from the selected format, then the same analysis pipeline processes it.

## Features

| File type | Reader | Content extracted |
| --- | --- | --- |
| `.txt` | `TxtFileReader` | Plain text using UTF-8 as the default encoding |
| `.docx` | `DocxFileReader` | Paragraphs in the main body of the Word document |
| `.pdf` | `PdfFileReader` | Text that can be extracted from the pages. Image-only scans require OCR before analysis. |

- **Word analysis.** The analyzer lowercases words using Turkish culture rules (`tr-TR`) and strips suffixes after an apostrophe in words such as `Bursa’ya`. It excludes tokens made up entirely of digits and Turkish stop words such as `ve`, `ile`, and `ama` from the word counts. The stop word list is defined in [`StopWords.cs`](Analysis/StopWords.cs).
- **Word statistics.** The report shows total and distinct word counts after filtering. It lists words that occur at least twice in descending order of frequency, with ties sorted in Turkish alphabetical order.
- **Punctuation analysis.** The analyzer counts each punctuation character in the extracted text, including apostrophes used to separate suffixes.
- **Error handling.** The console shows separate messages for unsupported file extensions and read failures. For read failures, it also displays the underlying error message when available.
- **Logging.** The application records file selection, reading, analysis, and errors at the `INFO` or `ERROR` level in `Logs/log_yyyy-MM-dd.txt`. It searches upward from the executable directory for a `.csproj` file and uses the `Logs/` folder in the first matching directory. If the search finds none, it uses a `Logs/` folder next to the executable.

## Usage

1. Open `FileAnalyzer.slnx` from the cloned `FileAnalyzer` repository in Visual Studio and press `Ctrl+F5` to start the application.
2. Select a `.txt`, `.docx`, or `.pdf` file in the file picker.
3. Review the file path, word summary, repeated words, and punctuation counts in the console.
4. Press any key to exit.

Example console output for the input `Elma, elma ve armut! 123`

```text
Word Summary (numbers and conjunctions excluded)
  Analyzed words: 3
  Distinct words: 2

Repeated Words
  elma                     2

Punctuation Marks
  '!'     1
  ','     1
```

## Architecture

The project is a single console application. Separate classes handle file reading, analysis, reporting, and logging. [`Program.cs`](Program.cs) brings these components together and controls the order of operations.

| Stage | Component | Responsibility |
| --- | --- | --- |
| File selection | `Program`, `OpenFileDialog` | Opens the dialog from the entry point marked with `[STAThread]` and builds its extension filter from `FileReaderFactory.SupportedExtensions`. |
| Reader selection and text extraction | `FileReaderFactory`, `IFileReader` | The factory matches the extension case-insensitively. The selected reader returns the extracted text as a `string` through `ReadText`. |
| Analysis | `TextAnalyzer`, `PunctuationAnalyzer` | Each analyzer processes the same text independently and writes its results to the shared `AnalysisResult` object. |
| Reporting | `ConsoleReporter` | Prints the `AnalysisResult` in sections and includes words with a frequency greater than one in the repeated words list. |
| Logging | `ILogger`, `FileLogger` | `Program` logs through `ILogger`, and `FileLogger` appends entries to the dated log file. |

To add support for a file format, implement `IFileReader` and register the new reader in `FileReaderFactory`. The file picker uses that same list to include the new extension in its filter.

## Known Limitations

- **Turkish casing for all words.** Words are lowercased with Turkish rules, so uppercase English words containing `I` are converted with a dotless `ı` (for example, `API` → `apı`).
- **Hyphenated words are split.** `e-ticaret` is counted as two tokens: `e` and `ticaret`.
- **Only apostrophe suffixes are removed.** `Ahmet'in` and `Ahmet'e` are both counted as `ahmet`, but suffixes written without an apostrophe are not stemmed (`kitap`, `kitabı`, and `kitaplar` are counted separately).
- **Context-free stop words.** Words with several meanings, such as `ne`, `gerek`, and `ister`, are always excluded, even when they are not used as conjunctions.
- **Text encoding.** `.txt` files are read as UTF-8. Files saved in a legacy encoding such as Windows-1254 may display incorrect Turkish characters.
- **Scanned PDFs.** PDFs that contain only images have no text layer and produce an empty report.
- **Word headers and footers.** Only the main body of `.docx` files is analyzed.

## Technologies

| Technology | Usage |
| --- | --- |
| C# and .NET Framework 4.8 | Windows console application |
| Windows Forms (`System.Windows.Forms`) | File selection through `OpenFileDialog` |
| `System.IO` | Plain-text reading and log file writing |
| `DocumentFormat.OpenXml` 3.5.1 | Paragraph text extraction from Word documents |
| `PdfPig` 0.1.16 | Word extraction from PDF pages |
| Regex, `CultureInfo`, `Dictionary`, and LINQ | Token matching, Turkish case conversion, counting, filtering, and sorting |
| NuGet (`packages.config`) | Version management for external packages and dependencies |

## Setup

**Requirements**

- Windows and Git
- Visual Studio with `.slnx` support (such as Visual Studio 2026), the **.NET desktop development** workload, and the **.NET Framework 4.8 Targeting Pack** or [Developer Pack](https://learn.microsoft.com/dotnet/framework/install/guide-for-developers)

```powershell
git clone https://github.com/ahmetkamac16/FileAnalyzer.git
cd FileAnalyzer
```

1. Open `FileAnalyzer.slnx` in Visual Studio.
2. In Solution Explorer, right-click the solution and choose **Restore NuGet Packages**. NuGet restores the versions specified in [`packages.config`](packages.config).
3. Select **Build → Build Solution**.
4. Press `Ctrl+F5` to start the application. Depending on the build configuration, the executable is generated at `bin\Debug\FileAnalyzer.exe` or `bin\Release\FileAnalyzer.exe`.

## Folder Structure

```text
FileAnalyzer/
├── Analysis/
│   ├── PunctuationAnalyzer.cs
│   ├── StopWords.cs
│   └── TextAnalyzer.cs
├── Exceptions/
│   ├── FileReadException.cs
│   └── UnsupportedFileTypeException.cs
├── Logging/
│   ├── FileLogger.cs
│   └── ILogger.cs
├── Logs/
│   └── .gitkeep
├── Models/
│   └── AnalysisResult.cs
├── Properties/
│   └── AssemblyInfo.cs
├── Readers/
│   ├── DocxFileReader.cs
│   ├── FileReaderFactory.cs
│   ├── IFileReader.cs
│   ├── PdfFileReader.cs
│   └── TxtFileReader.cs
├── Reporting/
│   └── ConsoleReporter.cs
├── .gitignore
├── App.config
├── FileAnalyzer.csproj
├── FileAnalyzer.slnx
├── packages.config
├── Program.cs
└── README.md
```
