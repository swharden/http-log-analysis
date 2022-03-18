using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalysis
{
    internal record LogRecord
    {
        public string IP { get; init; } = string.Empty;
        public string Hostname { get; init; } = string.Empty;
        public DateTime DateTime { get; init; } = DateTime.MinValue;
        public string Method { get; init; } = string.Empty;
        public string Path { get; init; } = string.Empty;
        public string RequestType { get; init; } = string.Empty;
        public int ResponseCode { get; init; } = 0;
        public int ResponseSize { get; init; } = 0;
        public string Referrer { get; init; } = string.Empty;
        public string Agent { get; init; } = string.Empty;
        public string Tail { get; init; } = string.Empty;


        public static LogRecord FromLogLine(string line)
        {
            string[] parts = line.Split('"');

            if (parts.Length != 7)
                throw new InvalidOperationException("line must have six double quote characters");

            string[] partsZero = parts[0].Split(' ');
            string[] partsOne = parts[1].Split(' ');
            string[] partsTwo = parts[2].Trim().Split(' ');

            return new LogRecord()
            {
                IP = partsZero[0],
                Hostname = partsZero[1],
                DateTime = GetDateTime(parts),
                Method = partsOne[0],
                Path = partsOne.Length > 1 ? partsOne[1] : string.Empty,
                RequestType = partsOne.Length > 1 ? partsOne[2] : string.Empty,
                ResponseCode = int.Parse(partsTwo[0]),
                ResponseSize = int.Parse(partsTwo[1]),
                Referrer = parts[3],
                Agent = parts[5],
                Tail = parts[6],
            };
        }

        public static DateTime GetDateTime(string[] parts)
        {
            string dateRaw = parts[0].Split("[")[1].Split("+")[0];

            string[] dateStringParts = dateRaw.Split(":")[0].Split("/");
            string dateOnlyString = $"{dateStringParts[1]} {dateStringParts[0]} {dateStringParts[2]}";
            DateOnly dtOnly = DateOnly.Parse(dateOnlyString);

            string timeOnlyString = dateRaw.Split(":", 2)[1];
            TimeOnly tmOnly = TimeOnly.Parse(timeOnlyString);
            DateTime dt = dtOnly.ToDateTime(tmOnly);

            return dt;
        }
    }
}
