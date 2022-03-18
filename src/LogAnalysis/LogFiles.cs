using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalysis
{
    internal static class LogFiles
    {
        public static string LogFileFolder => Path.GetFullPath("logFiles");

        public static void CreateLogFileFolder()
        {
            if (!Directory.Exists(LogFileFolder))
                Directory.CreateDirectory(LogFileFolder);
        }

        public static void ExtractAll()
        {
            foreach (string filePath in Directory.GetFiles(LogFileFolder, "*.gz"))
            {
                using FileStream compressedFileStream = File.Open(filePath, FileMode.Open);
                using GZipStream decompressor = new(compressedFileStream, CompressionMode.Decompress);

                string outputFileName = Path.GetFileNameWithoutExtension(filePath) + ".txt";
                string outputFilePath = Path.Combine(LogFileFolder, outputFileName);
                using FileStream outputFileStream = File.Create(outputFilePath);
                decompressor.CopyTo(outputFileStream);
                Console.WriteLine(outputFilePath);
            }
        }

        public static void Decompress(string gzipFilePath)
        {
            using FileStream compressedFileStream = File.Open(gzipFilePath, FileMode.Open);
            using GZipStream decompressor = new(compressedFileStream, CompressionMode.Decompress);

            string outputFolder = Path.GetDirectoryName(gzipFilePath) ?? string.Empty;
            string outputFileName = Path.GetFileNameWithoutExtension(gzipFilePath) + ".txt";
            string outputFilePath = Path.Combine(outputFolder, outputFileName);
            using FileStream outputFileStream = File.Create(outputFilePath);
            decompressor.CopyTo(outputFileStream);
        }
    }
}
