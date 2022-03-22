using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalysis
{
    public static class CreateReport
    {
        public static void Error(int days)
        {
            CreateRecordReport(ReadErrorRecords(days), "error.html");
        }

        public static void Success(int days)
        {
            CreateRecordReport(ReadSuccessRecords(days), "success.html");
        }

        public static void Referrer(int days)
        {
            LogRecord[] records = ReadSuccessRecords(days)
                .Where(x => !string.IsNullOrWhiteSpace(x.Referrer))
                .Where(x => !x.Referrer.Contains("swharden.com/"))
                .Where(x => !x.Referrer.Contains("/swharden.com"))
                .Where(x => !x.Referrer.Contains("google.com/"))
                .Where(x => !x.Referrer.Contains("yahoo.com/"))
                .Where(x => !x.Referrer.Contains("bing.com/"))
                .Where(x => !x.Referrer.Contains("duckduckgo.com/"))
                .Where(x => !x.Referrer.Contains("youtube.com/"))
                .Where(x => !x.Referrer.Contains("/google."))
                .Where(x => !x.Referrer.Contains("/www.google."))
                .Where(x => !x.Referrer.Contains("/scottplot.net"))
                .Where(x => !x.Referrer.Contains("/www.scottplot.net"))
                .ToArray();

            IGrouping<string, LogRecord>[] refRecords = records
                .GroupBy(x => x.Referrer, x => x)
                .OrderBy(x => x.Count())
                .Reverse()
                .ToArray();

            CreateRecordReport(records, "referrers.html");
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

        private static void CreateRecordReport(LogRecord[] records, string filename)
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

            string title = filename.Split(".")[0];
            Page pg = new(title);
            pg.AddTable(rows.ToArray(), columns);
            pg.Save(errorFilePath);
            Console.WriteLine(errorFilePath);
        }
    }
}
