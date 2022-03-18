namespace LogAnalysis;

public static class Program
{
    public static void Main()
    {
        FtpSecrets secrets = new();
        FTP.DownloadLogs(secrets);
    }
}