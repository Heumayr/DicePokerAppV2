using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePokerAppV2.Dataobject
{
    public static class Logger
    {
        public static readonly string LoggingPath = "log.padata";
        public static readonly string LoggingTempPath = "templog.padata";

        static Logger()
        {
            if (!File.Exists(LoggingPath))
            {
                using var temp = File.Create(LoggingPath);
            }
        }

        public static void AppendLog(string logString)
        {
            using var sw = File.AppendText(LoggingPath);
            sw.WriteLineAsync($"{logString}");
        }

        public static void CopyLogToPath(string path)
        {
            File.Copy(LoggingPath, path, true);
        }

        public static void ResetLog()
        {
            using var file = File.Create(LoggingPath);
        }

        internal static void SaveLogTemporary()
        {
            File.Copy(LoggingPath, LoggingTempPath, true);
        }

        internal static bool ReplaceLogWithTempLog()
        {
            if (File.Exists(LoggingTempPath))
            {
                File.Copy(LoggingTempPath, LoggingPath, true);
                DeleteTempLog();
                return true;
            }

            return false;
        }

        internal static void DeleteTempLog()
        {
            if (File.Exists(LoggingTempPath))
            {
                File.Delete(LoggingTempPath);
            }
        }
    }
}
