using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;

namespace LogAnalysis
{
    internal static class FTP
    {
        public static void DownloadLogs()
        {
            FtpSecrets secrets = new();

            using FtpClient client = new(secrets.Hostname, secrets.Username, secrets.Password);
            client.EncryptionMode = FtpEncryptionMode.Explicit;
            client.ValidateAnyCertificate = true;
            client.Connect();

            LogFiles.CreateLogFileFolder();

            FtpListItem[] items = client.GetListing("/")
                .Where(x => x.Name.EndsWith(".gz"))
                .OrderBy(x => x.Name)
                .ToArray();

            foreach (FtpListItem item in items)
            {
                string gzipFilePath = Path.Combine(LogFiles.LogFileFolder, item.Name);

                if (File.Exists(gzipFilePath))
                {
                    long localFileSize = new FileInfo(gzipFilePath).Length;
                    if (item.Size == localFileSize)
                    {
                        Console.WriteLine($"Skipping: {item.Name} {item.Size}");
                        continue;
                    }
                }

                Console.WriteLine($"Downloading: {item.Name}");
                using FileStream fs = new(gzipFilePath, FileMode.CreateNew);
                client.Download(fs, item.FullName);
                fs.Close();

                Console.WriteLine($"Decompressing: {item.Name}");
                LogFiles.Decompress(gzipFilePath);
            }
        }
    }
}
