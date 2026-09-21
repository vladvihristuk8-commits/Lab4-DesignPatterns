using System;
using System.IO;

namespace Lab4.Patterns
{

    public sealed class Logger
    {

        private static Logger _instance;

        private static readonly object _lock = new object();

        private readonly string _logFilePath;

        private Logger()
        {
            _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "app.log");
        }

        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Logger();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Info(string message) => Write("INFO", message);
        public void Warning(string message) => Write("WARN", message);
        public void Error(string message) => Write("ERROR", message);

        private void Write(string level, string message)
        {
            string record =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
            Console.WriteLine(record);
            File.AppendAllText(_logFilePath, record + Environment.NewLine);
        }
    }
}
