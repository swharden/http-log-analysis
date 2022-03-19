using System.Text;

namespace LogAnalysis
{
    internal class Page
    {
        public readonly string Title;

        private StringBuilder Html = new();

        public Page(string title)
        {
            Title = title;
        }

        public void AddTable(string[][] rows, string[]? headers = null)
        {
            Html.AppendLine("<div class='table-responsive'>");
            Html.AppendLine("<table class='table table-striped table-hover'>");

            if (headers is not null)
            {
                Html.AppendLine("<thead>");
                Html.AppendLine("<tr>");
                foreach (string column in headers)
                    Html.AppendLine($"<th scope='col' class='text-nowrap'>{column}</th>");
                Html.AppendLine("</tr>");
                Html.AppendLine("</thead>");
            }

            Html.AppendLine("<tbody>");
            foreach (string[] columns in rows)
            {
                Html.AppendLine("<tr>");
                foreach (string column in columns)
                    Html.AppendLine($"<td scope='col' style='max-width: 200px;'>{column}</td>");
                Html.AppendLine("</tr>");
            }
            Html.AppendLine("</tbody>");

            Html.AppendLine("</table>");
            Html.AppendLine("</div>");
        }

        public void Save(string path)
        {
            string html = Template.Base(Title, Html.ToString());
            File.WriteAllText(path, html);
        }
    }
}
