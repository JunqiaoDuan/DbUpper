using DbUpper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbUpper.Services
{
    public class ConsoleLogger
    {
        public void WriteLine(LogType logType = LogType.Info, string? message = null, Exception? ex = null)
        {
            switch (logType)
            {
                case LogType.Debug:
                    Console.ResetColor();
                    Console.WriteLine(message);
                    break;
                case LogType.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(message);
                    Console.ResetColor();
                    break;
                case LogType.Warn:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(message);
                    Console.ResetColor();
                    break;
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(message);
                    Console.WriteLine(ex);
                    Console.ResetColor();
                    break;
                default:
                    Console.WriteLine(message);
                    break;
            }
        }
    }
}
