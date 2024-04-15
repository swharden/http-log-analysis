namespace HttpLogAnalyzer;

internal readonly struct RequestRecord
{
    public DateTime DateTime { get; }
    public string IP { get; }
    public string Host { get; }
    public string RequestType { get; }
    public string Url { get; }
    public string UrlWithoutQueryString => Url.Split("?")[0];
    public int ResponseCode { get; }
    public int ResponseSize { get; }
    public string Referrer { get; }
    public string Client { get; }

    public RequestRecord(string line)
    {
        IP = line.Split(" ", 2)[0];
        Host = line.Split(" ", 3)[1];
        int startQuote1 = line.IndexOf('"', 0) + 1;
        string dateString = line[..startQuote1].Split("[")[1].Split("]")[0].Split(" ")[0];
        string[] dateParts = dateString.Replace("/", ":").Split(":");
        string dateString2 = $"{dateParts[1]} {dateParts[0]}, {dateParts[2]} " +
            $"{dateParts[3]}:{dateParts[4]}:{dateParts[5]}Z";
        DateTime = DateTime.Parse(dateString2);
        int endQuote1 = line.IndexOf('"', startQuote1);
        string quote1 = line[startQuote1..endQuote1];
        RequestType = quote1.Split(" ")[0];
        Url = quote1.Contains(' ') ? quote1.Split(" ")[1] : string.Empty;
        string line2 = line[(endQuote1 + 2)..];
        int space1 = line2.IndexOf(' ');
        int quote3 = line2.IndexOf('"');
        int quote4 = line2.IndexOf('"', quote3 + 1);
        ResponseCode = int.Parse(line2[..space1]);
        ResponseSize = int.Parse(line2[(space1 + 1)..quote3]);
        Referrer = line2[(quote3 + 1)..quote4];
        Client = line2[(quote4 + 2)..];
    }

    public void Show()
    {
        Console.WriteLine();
        Console.WriteLine($"IP: {IP}");
        Console.WriteLine($"Host: {Host}");
        Console.WriteLine($"Req: {RequestType}");
        Console.WriteLine($"Url: {Url}");
        Console.WriteLine($"code: {ResponseCode} ({GetStatusCodeName(ResponseCode)})");
        Console.WriteLine($"size: {ResponseSize}");
        Console.WriteLine($"referrer: {Referrer}");
        Console.WriteLine($"client: {Client}");
    }

    // TODO: put this on gist
    public static string GetStatusCodeName(int code) => code switch
    {
        100 => "Continue",
        101 => "Switching Protocols",
        102 => "Processing",
        103 => "Early Hints",
        200 => "OK",
        201 => "Created",
        202 => "Accepted",
        203 => "Non-Authoritative Information",
        204 => "No Content",
        205 => "Reset Content",
        206 => "Partial Content",
        207 => "Multi-Status",
        208 => "Already Reported",
        226 => "IM Used",
        300 => "Multiple Choices",
        301 => "Moved Permanently",
        302 => "Found",
        303 => "See Other",
        304 => "Not Modified",
        307 => "Temporary Redirect",
        308 => "Permanent Redirect",
        400 => "Bad Request",
        401 => "Unauthorized",
        402 => "Payment Required",
        403 => "Forbidden",
        404 => "Not Found",
        405 => "Method Not Allowed",
        406 => "Not Acceptable",
        407 => "Proxy Authentication Required",
        408 => "Request Timeout",
        409 => "Conflict",
        410 => "Gone",
        411 => "Length Required",
        412 => "Precondition Failed",
        413 => "Content Too Large",
        414 => "URI Too Long",
        415 => "Unsupported Media Type",
        416 => "Range Not Satisfiable",
        417 => "Expectation Failed",
        421 => "Misdirected Request",
        422 => "Unprocessable Content",
        423 => "Locked",
        424 => "Failed Dependency",
        425 => "Too Early",
        426 => "Upgrade Required",
        428 => "Precondition Required",
        429 => "Too Many Requests",
        431 => "Request Header Fields Too Large",
        451 => "Unavailable for Legal Reasons",
        500 => "Internal Server Error",
        501 => "Not Implemented",
        502 => "Bad Gateway",
        503 => "Service Unavailable",
        504 => "Gateway Timeout",
        505 => "HTTP Version Not Supported",
        506 => "Variant Also Negotiates",
        507 => "Insufficient Storage",
        508 => "Loop Detected",
        511 => "Network Authentication Required",
        _ => throw new NotImplementedException($"Unknown code: {code}"),
    };
}
