using System.Text;

namespace LogAnalysis;

public static class Program
{
    public static void Main()
    {
        //FTP.DownloadLogs();

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

        LogRecord[] errorRecords = LogFiles.GetLatestLogRecords()
            .Where(x => !ignoredCodes.Contains(x.ResponseCode))
            .ToArray();

        MakeReport(errorRecords);
    }

    private static void MakeReport(LogRecord[] records, string filename = "error.html")
    {
        StringBuilder sb = new();

        sb.AppendLine("<table>");
        foreach (LogRecord r in records)
        {
            sb.AppendLine("<tr>");
            sb.AppendLine($"<td>{r.DateTime}</td>");
            sb.AppendLine($"<td>{r.ResponseCode}</td>");
            sb.AppendLine($"<td>{r.Path}</td>");
            sb.AppendLine($"<td>{r.Referrer}</td>");
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</table>");
        
        File.WriteAllText(filename, sb.ToString());
        Console.WriteLine(Path.GetFullPath(filename));
    }
}