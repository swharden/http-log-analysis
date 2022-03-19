using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalysis
{
    internal static class Template
    {
        public static string Base(string title, string content)
        {
            StringBuilder sb = new();
            sb.AppendLine("<!doctype html>");
            sb.AppendLine("<html lang='en'>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset='utf-8'>");
            sb.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1'>");
            sb.AppendLine("<link href='https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css' rel='stylesheet'>");
            sb.AppendLine("<title>" + title + "</title>");

            sb.AppendLine("<style>");
            sb.AppendLine("a { text-decoration: none; }");
            sb.AppendLine("a:hover { text-decoration: underline; }");
            sb.AppendLine("</style>");

            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine(content);
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }
    }
}
