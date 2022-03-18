using System;
using System.Collections.Generic;
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
    }
}
