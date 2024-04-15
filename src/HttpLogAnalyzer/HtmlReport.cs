using System.Diagnostics;
using System.Text;

namespace HttpLogAnalyzer;

internal static class HtmlReport
{
    const string TEMPLATE =
        """
        <!doctype html>
        <html lang="en">
          <head>
            <meta charset="utf-8">
            <meta name="viewport" content="width=device-width, initial-scale=1">
            <title>{{TITLE}}</title>
            <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">
            <style>
            a { text-decoration: none; }
            a:hover { text-decoration: underline; }
            </style>
          </head>
          <body>
            {{CONTENT}}
            <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" crossorigin="anonymous"></script>
          </body>
        </html>
        """;

    public static void AllRequests(IEnumerable<RequestRecord> records, string filePath, bool launch = false)
    {
        StringBuilder sb = new();

        sb.AppendLine("<table class='table table-striped'>");
        sb.AppendLine("<tbody>");
        foreach (RequestRecord record in records)
        {
            sb.AppendLine("<tr>");
            sb.AppendLine($"<td>{record.DateTime.ToShortDateString()} {record.DateTime.ToShortTimeString()}</td>");
            sb.AppendLine($"<td>{record.IP}</td>");
            sb.AppendLine($"<td>{record.ResponseCode}</td>");
            sb.AppendLine($"<td><a href='https://{record.Host}{record.UrlWithoutQueryString}'>{record.Host}{record.UrlWithoutQueryString}</a></td>");
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</tbody>");
        sb.AppendLine("</table>");

        string html = TEMPLATE;
        html = html.Replace("{{TITLE}}", "Report");
        html = html.Replace("{{CONTENT}}", sb.ToString());

        filePath = Path.GetFullPath(filePath);
        File.WriteAllText(filePath, html);
        Console.WriteLine($"Saved: {filePath}");

        if (launch)
        {
            ProcessStartInfo psi = new(filePath) { UseShellExecute = true };
            Process p = new() { StartInfo = psi };
            p.Start();
        }
    }

    public static void TopPages(IEnumerable<RequestRecord> records, string filePath, int limit = 100, bool launch = false)
    {
        Dictionary<string, int> counts = [];

        foreach (RequestRecord record in records)
        {
            if (!counts.ContainsKey(record.UrlWithoutQueryString))
                counts[record.UrlWithoutQueryString] = 0;
            counts[record.UrlWithoutQueryString]++;
        }

        string[] orderedPages = counts
            .Select(x => (x.Key, x.Value))
            .OrderByDescending(x => x.Value)
            .Select(x => x.Key)
            .Take(limit)
            .ToArray();

        StringBuilder sb = new();

        sb.AppendLine("<table class='table table-striped'>");
        sb.AppendLine("<tbody>");
        foreach (string page in orderedPages)
        {
            int count = counts[page];
            sb.AppendLine("<tr>");
            sb.AppendLine($"<td>{count}</td>");
            sb.AppendLine($"<td><a href='https://swharden.com{page}'>https://swharden.com{page}</a></td>");
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</tbody>");
        sb.AppendLine("</table>");

        string html = TEMPLATE;
        html = html.Replace("{{TITLE}}", "Report");
        html = html.Replace("{{CONTENT}}", sb.ToString());

        filePath = Path.GetFullPath(filePath);
        File.WriteAllText(filePath, html);
        Console.WriteLine($"Saved: {filePath}");

        if (launch)
        {
            ProcessStartInfo psi = new(filePath) { UseShellExecute = true };
            Process p = new() { StartInfo = psi };
            p.Start();
        }
    }
}
