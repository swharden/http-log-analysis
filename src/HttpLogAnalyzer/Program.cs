using System.Diagnostics;
using System.IO.Compression;

namespace HttpLogAnalyzer;

internal class Program
{
    static void Main()
    {
        string gzFolderPath = @"C:\Users\swharden\Downloads\logs2\logs";
        string[] gzFilePaths = System.IO.Directory.GetFiles(gzFolderPath, "*.gz");

        Stopwatch sw = Stopwatch.StartNew();

        RecordCollection records = new();

        for (int i = 0; i < gzFilePaths.Length; i++)
        {
            Console.WriteLine($"Analyzing {i + 1} of {gzFilePaths.Length}...");
            records.AddLogFile(gzFilePaths[i]);
        }

        Console.WriteLine($"Loaded {records.Count:N0} records " +
            $"from {gzFilePaths.Length} files " +
            $"in {sw.Elapsed.TotalSeconds:N2} sec");

        records.KeepOnlyResponseCode(200);
        records.FilterOutCommonFileExtensions();
        records.FilterOutPrefix("/admin/");
        records.FilterOutPrefix("/weather/");
        records.FilterOutPrefix("/analytics/");
        records.FilterOutPrefix("/blog/feed/");

        Console.WriteLine($"{records.Count:N0} records remain after filtering");
        //HtmlReport.AllRequests(records.Records.Take(100), "records.html", true);
        HtmlReport.TopPages(records.Records, "pages.html", 1000, true);
    }
}