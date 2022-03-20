using System.Text;

namespace LogAnalysis;

public static class Program
{
    public static void Main()
    {
        FTP.DownloadLogs();
        CreateReport.Error();
        CreateReport.Success();
        CreateReport.Referrer();
    }
}