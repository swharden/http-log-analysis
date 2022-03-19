using System.Text;

namespace LogAnalysis;

public static class Program
{
    public static void Main()
    {
        CreateReport(ReadErrorRecords(3), "error.html");
        CreateReport(ReadSuccessRecords(3), "success.html");
    }

    private static LogRecord[] ReadSuccessRecords(int maxFileCount)
    {
        return LogFiles.LoadRecords(maxFileCount).Where(x => x.ResponseCode == 200).ToArray();
    }

    private static LogRecord[] ReadErrorRecords(int maxFileCount)
    {
        int[] ignoredCodes =
        {
            200, // OK
            301, // permanent redirect
            302, // temporary redirect
            304, // cached
            206, // partial (e.g., video buffering)
            499, // nginx processed but client aborted
            400, // refuse to process
            403, // forbidden
            401, // unauthorized
            //503, // server not ready to process
            //405, // method not allowed
            //404
        };

        return LogFiles.LoadRecords(maxFileCount).Where(x => !ignoredCodes.Contains(x.ResponseCode)).ToArray();
    }

    private static void CreateReport(LogRecord[] records, string filename = "report.html")
    {
        string errorFilePath = Path.GetFullPath(filename);

        List<string[]> rows = new();
        IGrouping<string, LogRecord>[] groups = records
            .GroupBy(x => x.Path, x => x)
            .OrderBy(x => x.Count())
            .Reverse()
            .ToArray();

        string[] columns = { "count", "URL", "Referrers" };
        foreach (var group in groups)
        {
            string url = group.First().Path;
            string fullUrl = "https://swharden.com" + url;
            string urlHtml = $"<a href='{fullUrl}'>{url}</a>";

            string refListHtml = string.Join(" ",
                group.Select(x => x.Referrer)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .OrderBy(x => x)
                .Select(x => $"<li><a href='{x}'>{x}</a></li>"));

            refListHtml = $"<div style='font-size: .8em;'>{refListHtml}</div>";

            string[] row = { group.Count().ToString(), urlHtml, refListHtml };
            rows.Add(row);
        }

        Page pg = new("Error Log");
        pg.AddTable(rows.ToArray(), columns);
        pg.Save(errorFilePath);
        Console.WriteLine(errorFilePath);
    }
}