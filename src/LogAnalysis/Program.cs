using System.Text;

namespace LogAnalysis;

public static class Program
{
    public static void Main(string[] args)
    {
        int days = (args.Length == 0) ? 1 : int.Parse(args[0]);
        FTP.DownloadLogs();
        CreateReport.Error(days);
        CreateReport.Success(days);
        CreateReport.Referrer(days);
    }
}