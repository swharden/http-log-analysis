using System.IO.Compression;

namespace HttpLogAnalyzer;

internal class RecordCollection
{
    public RequestRecord[] Records { get; private set; } = [];
    public int Count => Records.Length;

    public void AddLogFile(string filePath)
    {
        using FileStream fileStream = File.OpenRead(filePath);
        using GZipStream gzipStream = new(fileStream, CompressionMode.Decompress);
        using StreamReader reader = new(gzipStream);

        List<RequestRecord> records2 = [];
        while (!reader.EndOfStream)
        {
            string? line = reader.ReadLine();
            if (line is null)
                break;
            RequestRecord req = new(line);
            records2.Add(req);
        }
        Records = [.. Records, .. records2];
    }

    public void FilterOutCommonFileExtensions()
    {
        string[] extensions = [
            ".jpg",
            ".jpeg",
            ".png",
            ".svg",
            ".gif",
            ".css",
            ".js",
            ".ico",
            ".txt",
            ".webm",
            ".zip",
            ".webmanifest",
            ".json",
        ];

        foreach (string ext in extensions)
        {
            FilterOutFileExtension(ext);
        }
    }

    public void FilterOutFileExtension(string extension)
    {
        Records = Records
            .Where(x => !x.UrlWithoutQueryString.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }

    public void FilterOutPrefix(string prefix)
    {
        Records = Records
            .Where(x => !x.UrlWithoutQueryString.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }

    public void KeepOnlyResponseCode(int code)
    {
        Records = Records.Where(x => x.ResponseCode == code).ToArray();
    }

    public void FilterOutResponseCode(int code)
    {
        Records = Records.Where(x => x.ResponseCode != code).ToArray();
    }
}
